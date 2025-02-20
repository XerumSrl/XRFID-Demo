﻿using System.ComponentModel.DataAnnotations;

namespace XRFID.Server.Demo.V2.ViewModels;

public class AddLoadingUnitModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Code { get; set; } = string.Empty;

    [Required]
    public string Reference { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
