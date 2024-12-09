using Xerum.XFramework.Common.Dto;
using Xerum.XFramework.Common.Enums;

namespace XRFID.Demo.Common.Dto;

public class ProductDto : BaseDto
{
    public string? Description { get; set; }

    public string? Type { get; set; }

    public string? Note { get; set; }

    public string? BatchReference { get; set; }

    public string? Attrib1 { get; set; }

    public string? Attrib2 { get; set; }

    public string? Attrib3 { get; set; }

    public ItemStatus Status { get; set; } = ItemStatus.None;

    public string? Epc { get; set; }

    public string Regex { get; set; } = ".*";

    public string? SerialNumber { get; set; }

    public string? Barcode { get; set; }

    public int ContentQuantity { get; set; } = 1;

    public string? ContentUom { get; set; }

    public string? ContentQuality { get; set; }


    //soft delete
    public bool IsDeleted { get; set; } = false;



    #region Data Relations
    public ICollection<MovementItemDto> MovementItems { get; set; } = [];

    #endregion
}