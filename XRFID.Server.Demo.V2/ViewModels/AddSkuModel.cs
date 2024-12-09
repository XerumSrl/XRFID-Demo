using System.ComponentModel.DataAnnotations;

namespace XRFID.Server.Demo.V2.ViewModels;

public class AddSkuModel
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Reference { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime? StartDate { get; set; } = DateTime.Now;

    [Required]
    public DateTime? ExpirationDate { get; set; } = DateTime.Today.AddYears(1);
}