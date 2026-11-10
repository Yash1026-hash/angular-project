namespace CMMS.Api.Data.Entities;

public class WoPartsUsed
{
    public int Id { get; set; }

    public int WorkOrderId { get; set; }

    public int SparePartId { get; set; }

    public int Qty { get; set; }

    public decimal UnitCost { get; set; }

    public DateTime UsedAt { get; set; } = DateTime.UtcNow;
}