using System.Runtime.Serialization;

namespace XRFID.Server.Demo.V2.Entities.ReaderCfg;

/// <summary>
/// Gets or Sets ReportDirection
/// </summary>
public enum ReportDirectionEnum
{
    /// <summary>
    /// Enum INEnum for IN
    /// </summary>
    [EnumMember(Value = "IN")]
    IN = 0,
    /// <summary>
    /// Enum OUTEnum for OUT
    /// </summary>
    [EnumMember(Value = "OUT")]
    OUT = 1,
    /// <summary>
    /// Enum NONEEnum for NONE
    /// </summary>
    [EnumMember(Value = "NONE")]
    NONE = 2,
    /// <summary>
    /// Enum UNKNOWNEnum for UNKNOWN
    /// </summary>
    [EnumMember(Value = "UNKNOWN")]
    UNKNOWN = 3,
    /// <summary>
    /// Enum ERROREnum for ERROR
    /// </summary>
    [EnumMember(Value = "ERROR")]
    ERROR = 4
}

