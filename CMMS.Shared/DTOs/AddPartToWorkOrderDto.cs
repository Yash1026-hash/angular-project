namespace CMMS.Shared.DTOs.PmSchedules;

public class AddPartToWorkOrderDto
{
    public int TechnicianId { get; set; }

    public int SparePartId { get; set; }

    public int Qty { get; set; }
}