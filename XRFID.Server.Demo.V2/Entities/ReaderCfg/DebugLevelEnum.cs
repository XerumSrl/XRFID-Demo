using System.Runtime.Serialization;

namespace XRFID.Server.Demo.V2.Entities.ReaderCfg;

/// <summary>
/// 'Set the debug logging level '
/// </summary>
/// <value>'Set the debug logging level '</value>
public enum DebugLevelEnum
{
    /// <summary>
    /// Enum ERROREnum for ERROR
    /// </summary>
    [EnumMember(Value = "ERROR")]
    ERROR = 0,
    /// <summary>
    /// Enum WARNINGEnum for WARNING
    /// </summary>
    [EnumMember(Value = "WARNING")]
    WARNING = 1,
    /// <summary>
    /// Enum INFOEnum for INFO
    /// </summary>
    [EnumMember(Value = "INFO")]
    INFO = 2,
    /// <summary>
    /// Enum DEBUGEnum for DEBUG
    /// </summary>
    [EnumMember(Value = "DEBUG")]
    DEBUG = 3
}
