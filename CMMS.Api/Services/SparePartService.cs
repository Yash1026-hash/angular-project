using CMMS.Api.Data;
using CMMS.Api.Data.Entities;
using CMMS.Shared.DTOs.SpareParts;
using Microsoft.EntityFrameworkCore;

namespace CMMS.Api.Services;

public class SparePartService
{
    private readonly AppDbContext _context;

    public SparePartService(AppDbContext context)
    {
        _context = context;
    }





    // Get all active spare parts
    public async Task<List<SparePartDto>> GetAllAsync()
    {
        return await _context.SpareParts
            .AsNoTracking()
            .Where(p => p.IsActive)
            .Select(p => new SparePartDto
            {
                Id = p.Id,
                PartCode = p.PartCode,
                Name = p.Name,
                Unit = p.Unit,
                StockQty = p.StockQty,
                ReorderLevel = p.ReorderLevel,
                UnitCost = p.UnitCost,
                IsActive = p.IsActive
            })
            .ToListAsync();
    }







    // Get one spare part by ID
    public async Task<SparePartDto?> GetByIdAsync(int id)
    {
        return await _context.SpareParts
            .AsNoTracking()
            .Where(p => p.Id == id && p.IsActive)
            .Select(p => new SparePartDto
            {
                Id = p.Id,
                PartCode = p.PartCode,
                Name = p.Name,
                Unit = p.Unit,
                StockQty = p.StockQty,
                ReorderLevel = p.ReorderLevel,
                UnitCost = p.UnitCost,
                IsActive = p.IsActive
            })
            .FirstOrDefaultAsync();
    }












    public async Task<SparePartDto> CreateAsync(CreateSparePartDto dto)
{
    // Check whether the part code already exists
    bool codeExists = await _context.SpareParts
        .AnyAsync(p => p.PartCode == dto.PartCode);

    if (codeExists)
    {
        throw new InvalidOperationException(
            "A spare part with this code already exists."
        );
    }

    // Create entity
    var sparePart = new SparePart
    {
        PartCode = dto.PartCode,
        Name = dto.Name,
        Unit = dto.Unit,
        StockQty = dto.StockQty,
        ReorderLevel = dto.ReorderLevel,
        UnitCost = dto.UnitCost,
        IsActive = true,
        CreatedAt = DateTime.UtcNow
    };

    // Save to database
    _context.SpareParts.Add(sparePart);
    await _context.SaveChangesAsync();

    // Return DTO
    return new SparePartDto
    {
        Id = sparePart.Id,
        PartCode = sparePart.PartCode,
        Name = sparePart.Name,
        Unit = sparePart.Unit,
        StockQty = sparePart.StockQty,
        ReorderLevel = sparePart.ReorderLevel,
        UnitCost = sparePart.UnitCost,
        IsActive = sparePart.IsActive
    };
}









public async Task<SparePartDto?> UpdateAsync(
    int id,
    UpdateSparePartDto dto)
{
    var part = await _context.SpareParts
        .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

    if (part == null)
        return null;

    bool codeExists = await _context.SpareParts
        .AnyAsync(p => p.PartCode == dto.PartCode
                    && p.Id != id);

    if (codeExists)
        throw new InvalidOperationException(
            "This part code already exists.");

    part.PartCode = dto.PartCode;
    part.Name = dto.Name;
    part.Unit = dto.Unit;
    part.ReorderLevel = dto.ReorderLevel;
    part.UnitCost = dto.UnitCost;

    await _context.SaveChangesAsync();

    return new SparePartDto
    {
        Id = part.Id,
        PartCode = part.PartCode,
        Name = part.Name,
        Unit = part.Unit,
        StockQty = part.StockQty,
        ReorderLevel = part.ReorderLevel,
        UnitCost = part.UnitCost,
        IsActive = part.IsActive
    };
}








public async Task<bool> DeleteAsync(int id)
{
    var part = await _context.SpareParts
        .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

    if (part == null)
        return false;

    part.IsActive = false;

    await _context.SaveChangesAsync();

    return true;
}




public async Task<SparePartDto?> AdjustStockAsync(
    int id,
    AdjustStockDto dto)
{
    var part = await _context.SpareParts
        .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

    if (part == null)
        return null;

    if (dto.Quantity == 0)
        throw new InvalidOperationException(
            "Adjustment quantity cannot be zero.");

    if (part.StockQty + dto.Quantity < 0)
        throw new InvalidOperationException(
            "Stock cannot be negative.");

    part.StockQty += dto.Quantity;

    await _context.SaveChangesAsync();

    return new SparePartDto
    {
        Id = part.Id,
        PartCode = part.PartCode,
        Name = part.Name,
        Unit = part.Unit,
        StockQty = part.StockQty,
        ReorderLevel = part.ReorderLevel,
        UnitCost = part.UnitCost,
        IsActive = part.IsActive
    };
}



public async Task<List<SparePartDto>> GetLowStockAsync()
{
    return await _context.SpareParts
        .AsNoTracking()
        .Where(p => p.IsActive && p.StockQty <= p.ReorderLevel)
        .Select(p => new SparePartDto
        {
            Id = p.Id,
            PartCode = p.PartCode,
            Name = p.Name,
            Unit = p.Unit,
            StockQty = p.StockQty,
            ReorderLevel = p.ReorderLevel,
            UnitCost = p.UnitCost,
            IsActive = p.IsActive
        })
        .ToListAsync();
}





}