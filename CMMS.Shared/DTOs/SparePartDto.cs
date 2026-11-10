namespace CMMS.Shared.DTOs.SpareParts;

public class SparePartDto
{
    public int Id { get; set; }
    public string PartCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public int StockQty { get; set; }
    public int ReorderLevel { get; set; }
    public decimal UnitCost { get; set; }
    public bool IsActive { get; set; }
}