﻿namespace XRFID.Server.Demo.V2.StateMachines.Shipment.Interfaces;

public interface ITagInStatus
{
    Guid CorrelationId { get; set; }
    Guid Id { get; set; }
    public bool Value { get; set; }
    public string Notes { get; set; }
    public DateTime? Timestamp { get; set; }
}
