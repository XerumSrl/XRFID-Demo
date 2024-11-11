using System.Runtime.Serialization;

namespace XRFID.Server.Demo.V2.Entities.ReaderCfg;


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
