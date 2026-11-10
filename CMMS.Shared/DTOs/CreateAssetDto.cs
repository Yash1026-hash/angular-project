
using System.ComponentModel.DataAnnotations;

namespace CMMS.Shared.DTOs;

public class CreateAssetDto
{
    [Required]
    [StringLength(50)]
    public string AssetCode { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    public string Location { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^(Low|Medium|High|Critical)$")]
    public string Criticality { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^(Running|Down|Maintenance)$")]
    public string Status { get; set; } = "Running";
}
