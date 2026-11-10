namespace CMMS.Api.Data.Entities;

public class WorkOrder
{
    public int Id { get; set; }

    public int AssetId { get; set; }

    public int? PmScheduleId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Type { get; set; } = "PM";

    public string Status { get; set; } = "Submitted";

    public string Priority { get; set; } = "Medium";

    public int RequestedBy { get; set; }


    public DateTime TargetDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? WorkNotes { get; set; }



    
    public int? AssignedTo { get; set; }

public DateTime? StartedAt { get; set; }

public decimal? LabourHours { get; set; }

public DateTime? CompletedAt { get; set; }
}