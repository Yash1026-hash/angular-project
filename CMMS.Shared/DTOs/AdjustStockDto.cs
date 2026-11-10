namespace CMMS.Shared.DTOs.SpareParts;

public class AdjustStockDto
{
    public int Quantity { get; set; }
    public string Reason { get; set; } = string.Empty;
}