using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using XRFID.Demo.Modules.Mqtt.Payloads;
using XRFID.Demo.Modules.Mqtt.Publishers;

namespace XRFID.Server.Demo.V2.Utilities;

public class CommandUtility(IZebraMqttCommandPublisher _zebraMqttCommandPublisher, ILogger<CommandUtility> _logger)
{

    /// <inheritdoc/>
    public async Task StartReading(Guid readerId, string hostName)
    {
        await _zebraMqttCommandPublisher.Publish(new RAWMQTTCommands
        {
            ReaderId = readerId,
            Topic = "mcmds",
            HostName = hostName,
            Command = "start",
            CommandId = Guid.NewGuid().ToString(),
            Payload = new VoidPayload()
        });
    }

    /// <inheritdoc/>
    public async Task StopReading(Guid readerId, string hostName)
    {
        await _zebraMqttCommandPublisher.Publish(new RAWMQTTCommands
        {
            ReaderId = readerId,
            Topic = "mcmds",
            HostName = hostName,
            Command = "stop",
            CommandId = Guid.NewGuid().ToString(),
            Payload = new VoidPayload()
        });
    }

    /// <inheritdoc/>
    public async Task ReportRaw(Guid readerId, string hostName, bool enabled)
    {
        await _zebraMqttCommandPublisher.Publish(new RAWMQTTCommands
        {
            ReaderId = readerId,
            Topic = "mcmds",
            HostName = hostName,
            Command = "set_mode",
            CommandId = Guid.NewGuid().ToString(),
            Payload = new SetModePayload
            {
                ModeSpecificSettings = new OperatingModeSpecificSettingsPayload()
                {
                    AdvancedConfig = new DirectionalityAdvancedConfigPayload()
                    {
                        ReportRaw = enabled
                    }


                },
                Type = "DIRECTIONALITY"

            }
        });
    }
}

[DataContract]
public class VoidPayload { }

public class SetModePayload
{
    /// <summary>
    /// ModeSpecificSettings
    /// </summary>
    [JsonPropertyName("modeSpecificSettings")]
    public OperatingModeSpecificSettingsPayload? ModeSpecificSettings { get; set; }

    [JsonPropertyName("type")]
    public required string Type { get; set; }

}

public class OperatingModeSpecificSettingsPayload
{

    /// <summary>
    /// Gets or Sets AdvancedConfig
    /// </summary>
    [JsonPropertyName("advancedConfig")]
    public DirectionalityAdvancedConfigPayload? AdvancedConfig { get; set; }
}

public partial class DirectionalityAdvancedConfigPayload
{
    /// <summary>
    /// Report (or do not report) raw bearing/location messages
    /// </summary>
    /// <value>Report (or do not report) raw bearing/location messages</value>

    [JsonPropertyName("report_raw")]
    public bool? ReportRaw { get; set; }


}


public partial class DirectionalityAdvancedConfigAarsPayload
{
    /// <summary>
    /// AAR IP Address and Port to connect to (e.g. 192.168.0.23:5160)
    /// </summary>
    /// <value>AAR IP Address and Port to connect to (e.g. 192.168.0.23:5160)</value>
    [JsonPropertyName("address")]
    public string? Address { get; set; }

    /// <summary>
    /// X Location (in feet) of ATR relative to other ATR
    /// </summary>
    /// <value>X Location (in feet) of ATR relative to other ATR</value>
    [JsonPropertyName("x")]
    public decimal? X { get; set; }

    /// <summary>
    /// Y Location (in feet) of ATR relative to other ATR
    /// </summary>
    /// <value>Y Location (in feet) of ATR relative to other ATR</value>
    [JsonPropertyName("y")]
    public decimal? Y { get; set; }

    /// <summary>
    /// Height (in feet) of ATR 
    /// </summary>
    /// <value>Height (in feet) of ATR </value>
    [JsonPropertyName("z")]
    public decimal? Z { get; set; }

    /// <summary>
    /// Degrees of offset of ATR from North
    /// </summary>
    /// <value>Degrees of offset of ATR from North</value>
    [Range(0, 359)]
    [JsonPropertyName("offset")]
    public decimal? Offset { get; set; }

}

public partial class DirectionalityBasicConfigPayload
{
    /// <summary>
    /// Gets or Sets BeamConfig
    /// </summary>

    //[JsonPropertyName("beamConfig")]
    //public OneOfdirectionalityBasicConfigBeamConfig? BeamConfig { get; set; }

    /// <summary>
    /// The width of the main zones (in feet)
    /// </summary>
    /// <value>The width of the main zones (in feet)</value>

    [JsonPropertyName("innerZoneWidth")]
    public decimal? InnerZoneWidth { get; set; }

    /// <summary>
    /// Orientation of tag (in degrees)
    /// </summary>
    /// <value>Orientation of tag (in degrees)</value>

    [JsonPropertyName("orientation")]
    public decimal? Orientation { get; set; }

    /// <summary>
    /// Height of reader (in feet)
    /// </summary>
    /// <value>Height of reader (in feet)</value>

    [Range(10, 20)]
    [JsonPropertyName("readerHeight")]
    public decimal? ReaderHeight { get; set; }

    /// <summary>
    /// Height of tag (in feet)
    /// </summary>
    /// <value>Height of tag (in feet)</value>

    [Range(0, 20)]
    [JsonPropertyName("tagHeight")]
    public decimal? TagHeight { get; set; }

    /// <summary>
    /// The extension of the main zone into one another (in feet)
    /// </summary>
    /// <value>The extension of the main zone into one another (in feet)</value>

    [JsonPropertyName("zoneExtension")]
    public decimal? ZoneExtension { get; set; }

    /// <summary>
    /// User defined names to zones. Array must have the same number of zones as specified in the zone plan
    /// </summary>
    /// <value>User defined names to zones. Array must have the same number of zones as specified in the zone plan</value>

    [JsonPropertyName("zoneNames")]
    public List<string>? ZoneNames { get; set; }

    /// <summary>
    /// 4 or 6 zone zone plan
    /// </summary>
    /// <value>4 or 6 zone zone plan</value>
    public enum ZonePlanEnum
    {
        /// <summary>
        /// Enum NUMBER_4 for 4
        /// </summary>
        [EnumMember(Value = "4")]
        NUMBER_4 = 0,
        /// <summary>
        /// Enum NUMBER_6 for 6
        /// </summary>
        [EnumMember(Value = "6")]
        NUMBER_6 = 1
    }

    /// <summary>
    /// 4 or 6 zone zone plan
    /// </summary>
    /// <value>4 or 6 zone zone plan</value>

    [JsonPropertyName("zonePlan")]
    public ZonePlanEnum? ZonePlan { get; set; }


}