using System.ComponentModel.DataAnnotations;

namespace XRFID.Demo.Server.ViewModels;

public class AddMobileReaderModel
{
    [Required]
    [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "GUID is not valid")]
    public string Id { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^[a-zA-Z0-9\-_]{1,63}$", ErrorMessage = "Hostname is invalid")]
    public string Hostname { get; set; } = string.Empty;
}
