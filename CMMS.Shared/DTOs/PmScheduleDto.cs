namespace CMMS.Shared.DTOs.PmSchedules;

public class PmScheduleDto
{
    public int Id { get; set; }
    public int AssetId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int FrequencyDays { get; set; }
    public int LeadDays { get; set; }
    public DateTime NextDueDate { get; set; }
    public DateTime? LastDoneDate { get; set; }
    public bool IsActive { get; set; }
}