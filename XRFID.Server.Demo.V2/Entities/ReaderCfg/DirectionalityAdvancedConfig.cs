using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XRFID.Server.Demo.V2.Entities.ReaderCfg;

[ComplexType]
public partial class DirectionalityAdvancedConfig : IEquatable<DirectionalityAdvancedConfig?>
{
    public DirectionalityAdvancedConfig()
    {
        Aars = [];
    }

    /// <summary>
    /// Interval between processing incoming raw messages (in seconds)
    /// </summary>
    /// <value>Interval between processing incoming raw messages (in seconds)</value>

    // [JsonPropertyName("background_processing_interval_seconds")]
    public double? BackgroundProcessingIntervalSeconds { get; set; }

    // [JsonPropertyName("debug_level")]
    public DebugLevelEnum? DebugLevel { get; set; }

    /// <summary>
    /// Set the distance (in feet) a tag must travel into a zone to be able to move back into previous zone
    /// </summary>
    /// <value>Set the distance (in feet) a tag must travel into a zone to be able to move back into previous zone</value>

    [Range(0, 50)]
    // [JsonPropertyName("hysteresis_feet")]
    public double? HysteresisFeet { get; set; }

    /// <summary>
    /// Moving Average Window Duration (in seconds). Must be less than tag_timeout_seconds
    /// </summary>
    /// <value>Moving Average Window Duration (in seconds). Must be less than tag_timeout_seconds</value>

    [Range(0, 3600)]
    // [JsonPropertyName("ma_duration_seconds")]
    public double? MaDurationSeconds { get; set; }

    /// <summary>
    /// Report (or do not report) \&quot;NEW\&quot; events
    /// </summary>
    /// <value>Report (or do not report) \&quot;NEW\&quot; events</value>

    // [JsonPropertyName("report_new")]
    public bool? ReportNew { get; set; }

    /// <summary>
    /// Duration to report tag updates even if no transition (value in seconds) (-1 is never report)
    /// </summary>
    /// <value>Duration to report tag updates even if no transition (value in seconds) (-1 is never report)</value>

    [Range(-1, 86400)]
    // [JsonPropertyName("report_update_duration_seconds")]
    public double? ReportUpdateDurationSeconds { get; set; }

    /// <summary>
    /// Report (or do not report) \&quot;TRANSITION\&quot; events
    /// </summary>
    /// <value>Report (or do not report) \&quot;TRANSITION\&quot; events</value>

    // [JsonPropertyName("report_transition")]
    public bool? ReportTransition { get; set; }

    /// <summary>
    /// Report (or do not report) \&quot;TIMED_OUT\&quot; events
    /// </summary>
    /// <value>Report (or do not report) \&quot;TIMED_OUT\&quot; events</value>

    // [JsonPropertyName("report_timed_out")]
    public bool? ReportTimedOut { get; set; }


    // [JsonPropertyName("report_direction")]
    public List<ReportDirectionEnum>? ReportDirection { get; set; }

    /// <summary>
    /// Report (or do not report) zone history in \&quot;TIMED_OUT\&quot; event
    /// </summary>
    /// <value>Report (or do not report) zone history in \&quot;TIMED_OUT\&quot; event</value>

    // [JsonPropertyName("report_zone_history")]
    public bool? ReportZoneHistory { get; set; }

    /// <summary>
    /// Report (or do not report) raw bearing/location messages
    /// </summary>
    /// <value>Report (or do not report) raw bearing/location messages</value>

    // [JsonPropertyName("report_raw")]
    public bool? ReportRaw { get; set; }

    /// <summary>
    /// Minimum duration until a tag is deemed \&quot;gone\&quot; (in seconds)
    /// </summary>
    /// <value>Minimum duration until a tag is deemed \&quot;gone\&quot; (in seconds)</value>

    [Range(0, 3600)]
    // [JsonPropertyName("tag_timeout_seconds_min")]
    public double? TagTimeoutSecondsMin { get; set; }

    /// <summary>
    /// Default duration until a tag is deemed \&quot;gone\&quot; (in seconds)
    /// </summary>
    /// <value>Default duration until a tag is deemed \&quot;gone\&quot; (in seconds)</value>

    [Range(0, 86400)]
    // [JsonPropertyName("tag_timeout_seconds_default")]
    public double? TagTimeoutSecondsDefault { get; set; }

    /// <summary>
    /// Maximum duration until a tag is deemed \&quot;gone\&quot; (in seconds)
    /// </summary>
    /// <value>Maximum duration until a tag is deemed \&quot;gone\&quot; (in seconds)</value>

    [Range(0, 86400)]
    // [JsonPropertyName("tag_timeout_seconds_max")]
    public double? TagTimeoutSecondsMax { get; set; }

    /// <summary>
    /// Multiple of standard deviation of time between reads to determine adaptive timeout.
    /// </summary>
    /// <value>Multiple of standard deviation of time between reads to determine adaptive timeout.</value>

    // [JsonPropertyName("sigma_multiplier")]
    public int? SigmaMultiplier { get; set; }

    /// <summary>
    /// ATR information for 2 ATR Directionality
    /// </summary>
    /// <value>ATR information for 2 ATR Directionality</value>

    // [JsonPropertyName("aars")]
    public List<DirectionalityAdvancedConfigAars>? Aars { get; set; }

    /// <summary>
    /// Maximum tags limit
    /// </summary>
    /// <value>Maximum tags limit</value>

    // [JsonPropertyName("max_tags_limit")]
    public int? MaxTagsLimit { get; set; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as DirectionalityAdvancedConfig);
    }

    public bool Equals(DirectionalityAdvancedConfig? other)
    {
        return other is not null &&
               BackgroundProcessingIntervalSeconds == other.BackgroundProcessingIntervalSeconds &&
               DebugLevel == other.DebugLevel &&
               HysteresisFeet == other.HysteresisFeet &&
               MaDurationSeconds == other.MaDurationSeconds &&
               ReportNew == other.ReportNew &&
               ReportUpdateDurationSeconds == other.ReportUpdateDurationSeconds &&
               ReportTransition == other.ReportTransition &&
               ReportTimedOut == other.ReportTimedOut &&
               EqualityComparer<List<ReportDirectionEnum>?>.Default.Equals(ReportDirection, other.ReportDirection) &&
               ReportZoneHistory == other.ReportZoneHistory &&
               ReportRaw == other.ReportRaw &&
               TagTimeoutSecondsMin == other.TagTimeoutSecondsMin &&
               TagTimeoutSecondsDefault == other.TagTimeoutSecondsDefault &&
               TagTimeoutSecondsMax == other.TagTimeoutSecondsMax &&
               SigmaMultiplier == other.SigmaMultiplier &&
               EqualityComparer<List<DirectionalityAdvancedConfigAars>?>.Default.Equals(Aars, other.Aars) &&
               MaxTagsLimit == other.MaxTagsLimit;
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(BackgroundProcessingIntervalSeconds);
        hash.Add(DebugLevel);
        hash.Add(HysteresisFeet);
        hash.Add(MaDurationSeconds);
        hash.Add(ReportNew);
        hash.Add(ReportUpdateDurationSeconds);
        hash.Add(ReportTransition);
        hash.Add(ReportTimedOut);
        hash.Add(ReportDirection);
        hash.Add(ReportZoneHistory);
        hash.Add(ReportRaw);
        hash.Add(TagTimeoutSecondsMin);
        hash.Add(TagTimeoutSecondsDefault);
        hash.Add(TagTimeoutSecondsMax);
        hash.Add(SigmaMultiplier);
        hash.Add(Aars);
        hash.Add(MaxTagsLimit);
        return hash.ToHashCode();
    }

    public static bool operator ==(DirectionalityAdvancedConfig? left, DirectionalityAdvancedConfig? right)
    {
        return EqualityComparer<DirectionalityAdvancedConfig>.Default.Equals(left, right);
    }

    public static bool operator !=(DirectionalityAdvancedConfig? left, DirectionalityAdvancedConfig? right)
    {
        return !(left == right);
    }
}
