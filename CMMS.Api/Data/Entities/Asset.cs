
namespace CMMS.Api.Data.Entities
{
    public class Asset
    {
        // Primary Key
        public int Id { get; set; }

        // Asset Information
        public string AssetCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

        // Classification
        public string Criticality { get; set; } = string.Empty;

        // Operational Status
        public string Status { get; set; } = "Running";

        // Record Status
        public bool IsActive { get; set; } = true;

        // Audit Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
