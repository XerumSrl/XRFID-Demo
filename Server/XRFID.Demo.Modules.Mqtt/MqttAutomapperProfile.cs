using AutoMapper;
using MQTTnet;
using XRFID.Demo.Modules.Mqtt.Contracts;

namespace XRFID.Demo.Modules.Mqtt;

public class MqttAutomapperProfile : Profile
{
    public MqttAutomapperProfile()
    {
        CreateMap<MqttApplicationMessage, ZebraMqttApplicationMessage>().ReverseMap().ForAllMembers(opt => opt.AllowNull());

        //CreateMap<OperatingModeSpecificSettingsPayload, OperatingModeSpecificSettingsMessage>();
        //CreateMap<DirectionalityAdvancedConfigPayload, DirectionalityAdvancedConfigMessage>();
        //CreateMap<DirectionalityBasicConfigPayload, DirectionalityBasicConfigMessage>();
        //CreateMap<DirectionalityAdvancedConfigAarsPayload, DirectionalityAdvancedConfigAarsMessage>();

        CreateMap<RawDirectionalityTagPayload, ZebraRawDirectionalityTagData>()
            .ForMember(d => d.IdHex, opt => opt.MapFrom(s => s.IdHex.ToUpper()));

        CreateMap<DirectionalityTagPayload, ZebraDirectionalityTagData>();
    }
}
