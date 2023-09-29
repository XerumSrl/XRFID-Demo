namespace XRFID.Demo.Server.Entities;

public abstract class AuditEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;

    public string? CreatorUserId { get; set; }
    public DateTime CreationTime { get; set; } = DateTime.Now;
    public string? LastModifierUserId { get; set; }
    public DateTime LastModificationTime { get; set; } = DateTime.Now;
}
