namespace CMMS.Shared.DTOs.SpareParts;

public class UpdateSparePartDto
{
    public string PartCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public int ReorderLevel { get; set; }
    public decimal UnitCost { get; set; }
}