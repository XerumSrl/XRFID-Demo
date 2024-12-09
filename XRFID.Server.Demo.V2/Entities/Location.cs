using Xerum.XFramework.DBAccess.Entities;

namespace XRFID.Server.Demo.V2.Entities;

public class Location : AuditEntity
{
    public string? Description { get; set; }

    public Guid? ParentLocationId { get; set; }

    /// <summary>
    /// if true operate readers in this location in unison
    /// </summary>
    public bool IsGroup { get; set; } = false;

    #region EF Logic Links

    public Location? ParentLocation { get; set; }

    public ICollection<Location> ChildLocations { get; set; } = [];

    public ICollection<Reader> Readers { get; set; } = [];

    public ICollection<Printer> Printers { get; set; } = [];

    #endregion
}
