namespace CMMS.Shared.DTOs.SpareParts;

public class CreateSparePartDto
{
    public string PartCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public int StockQty { get; set; }
    public int ReorderLevel { get; set; }
    public decimal UnitCost { get; set; }
}