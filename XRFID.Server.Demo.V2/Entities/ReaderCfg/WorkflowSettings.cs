using Xerum.XFramework.Common.Enums;

namespace XRFID.Server.Demo.V2.Entities.ReaderCfg;

public class WorkflowSettings : IEquatable<WorkflowSettings?>
{
    public int InventoryDuration { get; set; } = 5000;

    public int ExpectedItemsCount { get; set; } = 1;//NEVER READ?
    public MovementType MovementType { get; set; } = MovementType.Single;

    public MovementContentEnum MovementContent { get; set; } = MovementContentEnum.Product;

    public TagFoundFilter? TagFoundFilter { get; set; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as WorkflowSettings);
    }

    public bool Equals(WorkflowSettings? other)
    {
        return other is not null &&
               InventoryDuration == other.InventoryDuration &&
               ExpectedItemsCount == other.ExpectedItemsCount &&
               MovementType == other.MovementType &&
               MovementContent == other.MovementContent &&
               EqualityComparer<TagFoundFilter?>.Default.Equals(TagFoundFilter, other.TagFoundFilter);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(InventoryDuration, ExpectedItemsCount, MovementType, MovementContent, TagFoundFilter);
    }

    public static bool operator ==(WorkflowSettings? left, WorkflowSettings? right)
    {
        return EqualityComparer<WorkflowSettings>.Default.Equals(left, right);
    }

    public static bool operator !=(WorkflowSettings? left, WorkflowSettings? right)
    {
        return !(left == right);
    }
}