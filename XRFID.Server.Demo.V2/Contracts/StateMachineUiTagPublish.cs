using XRFID.Demo.Common.Dto;

namespace XRFID.Server.Demo.V2.Contracts;

public record StateMachineUiTagPublish
{
    public Guid ReaderId { get; set; }

    public Guid ActivMoveId { get; set; }

    public TagDto? Tag { get; set; }
}
