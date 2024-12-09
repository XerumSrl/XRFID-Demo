using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xerum.XFramework.Common.Enums;

namespace XRFID.Demo.Modules.Mqtt.Contracts;

[DataContract]
public class DirectionalityTagPayload
{
    /// <summary>
    /// Monotonically increasing number to uniquely indentify message
    /// </summary>
    /// <value>Monotonically increasing number to uniquely indentify message</value>
    [Required]

    [JsonPropertyName("eventNum")]
    public int? EventNum { get; set; }

    /// <summary>
    /// EPC (if GS1 format) or UII (if ISO format) of the tag in hex format
    /// </summary>
    /// <value>EPC (if GS1 format) or UII (if ISO format) of the tag in hex format</value>
    [JsonPropertyName("tagId")]
    public required string IdHex { get; set; }

    /// <summary>
    /// State of the tag
    /// </summary>
    /// <value>State of the tag</value>
    [JsonPropertyName("state")]
    public required TagStateEnum State { get; set; }

    /// <summary>
    /// Zone number of the tag
    /// </summary>
    /// <value>Zone number of the tag</value>
    [Range(1, 6)]
    [JsonPropertyName("zone")]
    public required short Zone { get; set; }

    /// <summary>
    /// User defined zone name
    /// </summary>
    /// <value>User defined zone name</value>
    [JsonPropertyName("zoneName")]
    public string? ZoneName { get; set; }

    /// <summary>
    /// The previous zone number of the tag. Only available for \&quot;TRANSITION\&quot; state
    /// </summary>
    /// <value>The previous zone number of the tag. Only available for \&quot;TRANSITION\&quot; state</value>
    [Range(1, 6)]
    [JsonPropertyName("prevZone")]
    public decimal? PrevZone { get; set; }

    /// <summary>
    /// The previous zone name of the tag. Only available for \&quot;TRANSITION\&quot; state
    /// </summary>
    /// <value>The previous zone name of the tag. Only available for \&quot;TRANSITION\&quot; state</value>
    [JsonPropertyName("prevZoneName")]
    public string? PrevZoneName { get; set; }

    /// <summary>
    /// Indicates the direction the tag traveled. Only included in the \&quot;TIMED_OUT\&quot; state.
    /// </summary>
    /// <value>Indicates the direction the tag traveled. Only included in the \&quot;TIMED_OUT\&quot; state.</value>
    [JsonPropertyName("direction")]
    public TagDirectionEnum? Direction { get; set; }

    /// <summary>
    /// An array of zones traversed. Only available in the \&quot;TIMED_OUT\&quot; state and when report_zone_history is enabled. Each entry of the array is an array with 2 values - the timestamp (UNIX millisecond timestamp) and zone number.
    /// </summary>
    /// <value>An array of zones traversed. Only available in the \&quot;TIMED_OUT\&quot; state and when report_zone_history is enabled. Each entry of the array is an array with 2 values - the timestamp (UNIX millisecond timestamp) and zone number.</value>
    [JsonPropertyName("zoneHistory")]
    public List<List<decimal>>? ZoneHistory { get; set; }

    /// <summary>
    /// An array of locations. Only available in the \&quot;TIMED_OUT\&quot; state when report_location_history is enabeld. Each entry of the array is an array with 3 values - the timestamp (UNIX millisecond timestamp) and the x and y location
    /// </summary>
    /// <value>An array of locations. Only available in the \&quot;TIMED_OUT\&quot; state when report_location_history is enabeld. Each entry of the array is an array with 3 values - the timestamp (UNIX millisecond timestamp) and the x and y location</value>

    [JsonPropertyName("locationHistory")]
    public List<List<decimal>>? LocationHistory { get; set; }

    /// <summary>
    /// User Defined Parameter. Supplied by end user. Can be string or any valid json object
    /// </summary>
    /// <value>User Defined Parameter. Supplied by end user. Can be string or any valid json object</value>
    [JsonPropertyName("user_defined")]
    public string? UserDefined { get; set; }

    /// <summary>
    /// Timestamp in ISO-8610 format indicating the last time the tag was seen. Only available in the \&quot;TIMED OUT\&quot; state.
    /// </summary>
    /// <value>Timestamp in ISO-8610 format indicating the last time the tag was seen. Only available in the \&quot;TIMED OUT\&quot; state.</value>
    [JsonPropertyName("lastSeenTimestamp")]
    public DateTimeOffset? LastSeenTimestamp { get; set; }
}
