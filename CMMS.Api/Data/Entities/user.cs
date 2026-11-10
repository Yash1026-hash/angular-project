
namespace CMMS.Api.Data.Entities
{
    public class User
    {
        // Primary Key
        public int Id { get; set; }

        // Basic Information
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Authentication
        public string PasswordHash { get; set; } = string.Empty;

        // Authorization
        public string Role { get; set; } = string.Empty;

        // Account Status
        public bool IsActive { get; set; } = true;

        // Audit Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
