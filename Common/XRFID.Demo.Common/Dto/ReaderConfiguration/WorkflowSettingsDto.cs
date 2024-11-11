using Xerum.XFramework.Common.Enums;

namespace XRFID.Demo.Common.Dto.ReaderConfiguration;

public class WorkflowSettingsDto : IEquatable<WorkflowSettingsDto?>
{

    public int InventoryDuration { get; set; } = 5000;

    public int ExpectedItemsCount { get; set; } = 1;//NEVER READ?
    public MovementType MovementType { get; set; } = MovementType.Single;

    public MovementContentEnum MovementContent { get; set; } = MovementContentEnum.Product;

    public TagFoundFilterDto? TagFoundFilter { get; set; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as WorkflowSettingsDto);
    }

    public bool Equals(WorkflowSettingsDto? other)
    {
        return other is not null &&
               InventoryDuration == other.InventoryDuration &&
               ExpectedItemsCount == other.ExpectedItemsCount &&
               MovementType == other.MovementType &&
               MovementContent == other.MovementContent &&
               EqualityComparer<TagFoundFilterDto?>.Default.Equals(TagFoundFilter, other.TagFoundFilter);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(InventoryDuration, ExpectedItemsCount, MovementType, MovementContent, TagFoundFilter);
    }

    public static bool operator ==(WorkflowSettingsDto? left, WorkflowSettingsDto? right)
    {
        return EqualityComparer<WorkflowSettingsDto>.Default.Equals(left, right);
    }

    public static bool operator !=(WorkflowSettingsDto? left, WorkflowSettingsDto? right)
    {
        return !(left == right);
    }
}