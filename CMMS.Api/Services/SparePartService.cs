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
}