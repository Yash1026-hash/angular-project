using CMMS.Api.Data;
using CMMS.Api.Data.Entities;
using CMMS.Shared.DTOs.PmSchedules;
using Microsoft.EntityFrameworkCore;

namespace CMMS.Api.Services;

public class PmScheduleService
{
    private readonly AppDbContext _context;

    public PmScheduleService(AppDbContext context)
    {
        _context = context;
    }

    // GET: All active PM schedules
    public async Task<List<PmScheduleDto>> GetAllAsync()
    {
        return await _context.PmSchedules
            .Where(p => p.IsActive)
            .Select(p => new PmScheduleDto
            {
                Id = p.Id,
                AssetId = p.AssetId,
                Title = p.Title,
                Description = p.Description,
                FrequencyDays = p.FrequencyDays,
                LeadDays = p.LeadDays,
                NextDueDate = p.NextDueDate,
                LastDoneDate = p.LastDoneDate,
                IsActive = p.IsActive
            })
            .ToListAsync();
    }

    // GET: PM schedule by ID
    public async Task<PmScheduleDto?> GetByIdAsync(int id)
    {
        return await _context.PmSchedules
            .Where(p => p.Id == id && p.IsActive)
            .Select(p => new PmScheduleDto
            {
                Id = p.Id,
                AssetId = p.AssetId,
                Title = p.Title,
                Description = p.Description,
                FrequencyDays = p.FrequencyDays,
                LeadDays = p.LeadDays,
                NextDueDate = p.NextDueDate,
                LastDoneDate = p.LastDoneDate,
                IsActive = p.IsActive
            })
            .FirstOrDefaultAsync();
    }

    // POST: Create PM schedule
    public async Task<PmScheduleDto> CreateAsync(CreatePmScheduleDto dto)
    {
        var schedule = new PmSchedule
        {
            AssetId = dto.AssetId,
            Title = dto.Title,
            Description = dto.Description,
            FrequencyDays = dto.FrequencyDays,
            LeadDays = dto.LeadDays,
            NextDueDate = dto.NextDueDate,
            IsActive = true
        };

        _context.PmSchedules.Add(schedule);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(schedule.Id)
            ?? throw new InvalidOperationException("Created schedule could not be found.");
    }

    // PUT: Update PM schedule
    public async Task<PmScheduleDto?> UpdateAsync(int id, UpdatePmScheduleDto dto)
    {
        var schedule = await _context.PmSchedules
            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

        if (schedule == null)
            return null;

        schedule.AssetId = dto.AssetId;
        schedule.Title = dto.Title;
        schedule.Description = dto.Description;
        schedule.FrequencyDays = dto.FrequencyDays;
        schedule.LeadDays = dto.LeadDays;
        schedule.NextDueDate = dto.NextDueDate;

        await _context.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    // DELETE: Soft delete PM schedule
    public async Task<bool> DeleteAsync(int id)
    {
        var schedule = await _context.PmSchedules
            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

        if (schedule == null)
            return false;

        schedule.IsActive = false;
        await _context.SaveChangesAsync();

        return true;
    }




    public async Task<int> GenerateWorkOrdersAsync(int requestedBy)
{
    var today = DateTime.Today;

    var schedules = await _context.PmSchedules
        .Where(p => p.IsActive &&
                    p.NextDueDate <= today.AddDays(p.LeadDays))
        .ToListAsync();

    int createdCount = 0;

    foreach (var schedule in schedules)
    {
        // Check whether an open work order already exists
        bool alreadyExists = await _context.WorkOrders
            .AnyAsync(w =>
                w.PmScheduleId == schedule.Id &&
                w.Status != "Closed" &&
                w.Status != "Cancelled");

        if (alreadyExists)
            continue;

        var workOrder = new WorkOrder
        {
            AssetId = schedule.AssetId,
            PmScheduleId = schedule.Id,
            Title = schedule.Title,
            Description = schedule.Description,
            Type = "PM",
            Status = "Submitted",
            Priority = "Medium",
            RequestedBy = requestedBy,
            TargetDate = schedule.NextDueDate,
            CreatedAt = DateTime.UtcNow
        };

        _context.WorkOrders.Add(workOrder);
        createdCount++;
    }

    await _context.SaveChangesAsync();

    return createdCount;
}



public async Task<bool> StartWorkOrderAsync(int workOrderId, int technicianId)
{
    var workOrder = await _context.WorkOrders
        .FirstOrDefaultAsync(w => w.Id == workOrderId);

    if (workOrder == null)
        throw new KeyNotFoundException("Work order not found.");

    if (workOrder.AssignedTo != technicianId)
        throw new UnauthorizedAccessException(
            "You are not assigned to this work order.");

    if (workOrder.Status != "Submitted" &&
        workOrder.Status != "Approved")
        throw new InvalidOperationException(
            "Work order cannot be started in its current status.");

    workOrder.Status = "InProgress";
    workOrder.StartedAt = DateTime.UtcNow;

    await _context.SaveChangesAsync();

    return true;
}


public async Task AddPartAsync(
    int workOrderId,
    AddPartToWorkOrderDto dto)
{
    if (dto.Qty <= 0)
        throw new ArgumentException("Quantity must be greater than zero.");

    await using var transaction =
        await _context.Database.BeginTransactionAsync();

    var workOrder = await _context.WorkOrders
        .FirstOrDefaultAsync(w => w.Id == workOrderId);

    if (workOrder == null)
        throw new KeyNotFoundException("Work order not found.");

    if (workOrder.AssignedTo != dto.TechnicianId)
        throw new UnauthorizedAccessException(
            "You are not assigned to this work order.");

    if (workOrder.Status != "InProgress")
        throw new InvalidOperationException(
            "Parts can only be added to an InProgress work order.");

    var part = await _context.SpareParts
        .FirstOrDefaultAsync(p =>
            p.Id == dto.SparePartId && p.IsActive);

    if (part == null)
        throw new KeyNotFoundException(
            "Spare part not found or inactive.");

    // Atomically deduct stock only if sufficient stock exists.
    int affectedRows = await _context.SpareParts
        .Where(p =>
            p.Id == dto.SparePartId &&
            p.IsActive &&
            p.StockQty >= dto.Qty)
        .ExecuteUpdateAsync(setters =>
            setters.SetProperty(
                p => p.StockQty,
                p => p.StockQty - dto.Qty));

    if (affectedRows == 0)
        throw new InvalidOperationException(
            "Insufficient stock available.");

    var partsUsed = new WoPartsUsed
    {
        WorkOrderId = workOrderId,
        SparePartId = dto.SparePartId,
        Qty = dto.Qty,
        UnitCost = part.UnitCost,
        UsedAt = DateTime.UtcNow
    };

    _context.WoPartsUsed.Add(partsUsed);

    await _context.SaveChangesAsync();
    await transaction.CommitAsync();
}


public async Task<bool> CompleteWorkOrderAsync(
    int workOrderId,
    CompleteWorkOrderDto dto)
{
    if (string.IsNullOrWhiteSpace(dto.WorkNotes))
        throw new ArgumentException("Work notes are required.");

    if (dto.LabourHours <= 0)
        throw new ArgumentException("Labour hours must be greater than zero.");

    var workOrder = await _context.WorkOrders
        .FirstOrDefaultAsync(w => w.Id == workOrderId);

    if (workOrder == null)
        throw new KeyNotFoundException("Work order not found.");

    if (workOrder.AssignedTo != dto.TechnicianId)
        throw new UnauthorizedAccessException(
            "You are not assigned to this work order.");

    if (workOrder.Status != "InProgress")
        throw new InvalidOperationException(
            "Only InProgress work orders can be completed.");

    workOrder.WorkNotes = dto.WorkNotes;
    workOrder.LabourHours = dto.LabourHours;
    workOrder.CompletedAt = DateTime.UtcNow;
    workOrder.Status = "Completed";

    await _context.SaveChangesAsync();

    return true;
}
}