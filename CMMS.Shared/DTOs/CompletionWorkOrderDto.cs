namespace CMMS.Shared.DTOs.PmSchedules;

public class CompleteWorkOrderDto
{
    public int TechnicianId { get; set; }

    public string WorkNotes { get; set; } = string.Empty;

    public decimal LabourHours { get; set; }
}