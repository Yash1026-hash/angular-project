namespace CMMS.Api.Data.Entities;
//represents how data is stored in the database.
public class SparePart
{
    public int Id { get; set; }

    public string PartCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Unit { get; set; } = string.Empty;

    public int StockQty { get; set; }

    public int ReorderLevel { get; set; }

    public decimal UnitCost { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}