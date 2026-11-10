
using System.ComponentModel.DataAnnotations;

namespace CMMS.Shared.DTOs;

public class CreateUserDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^(Admin|Supervisor|Technician|Operator)$")]
    public string Role { get; set; } = string.Empty;
}
