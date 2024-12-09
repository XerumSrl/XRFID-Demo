using AutoMapper;
using XRFID.Demo.Common.Dto;
using XRFID.Demo.Common.Dto.GPIOConfiguration;
using XRFID.Demo.Common.Dto.ReaderConfiguration;
using XRFID.Server.Demo.V2.Entities;
using XRFID.Server.Demo.V2.Entities.GPIOCfg;
using XRFID.Server.Demo.V2.Entities.ReaderCfg;
using XRFID.Server.Demo.V2.ViewModels;

namespace XRFID.Server.Demo.V2.Mapper;

public class XRFIDSampleProfile : Profile
{
    public XRFIDSampleProfile()
    {
        CreateMap<Movement, MovementDto>().ReverseMap().ForAllMembers(opt => opt.AllowNull());

        CreateMap<MovementItem, MovementItemDto>().ReverseMap().ForAllMembers(opt => opt.AllowNull());

        CreateMap<LoadingUnit, LoadingUnitDto>().ReverseMap().ForAllMembers(opt => opt.AllowNull());

        CreateMap<LoadingUnitItem, LoadingUnitItemDto>().ReverseMap().ForAllMembers(opt => opt.AllowNull());

        CreateMap<Reader, ReaderDto>().ReverseMap().ForAllMembers(opt => opt.AllowNull());

        CreateMap<Product, ProductDto>().ReverseMap().ForAllMembers(opt => opt.AllowNull());
        CreateMap<Printer, PrinterDto>().ReverseMap().ForAllMembers(opt => opt.AllowNull());
        CreateMap<Label, LabelDto>().ReverseMap().ForAllMembers(opt => opt.AllowNull());

        //repository <-> pagine
        CreateMap<Sku, SkuModel>().ReverseMap().ForAllMembers(opt => opt.AllowNull());
        CreateMap<Product, ProductModel>().ReverseMap().ForAllMembers(opt => opt.AllowNull());
        CreateMap<Product, PrintProductModel>().ForAllMembers(opt => opt.AllowNull());
        CreateMap<PrintProductModel, Product>().ForAllMembers(opt => opt.AllowNull());
        CreateMap<Label, LabelModel>().ReverseMap().ForAllMembers(opt => opt.AllowNull());
        CreateMap<Printer, PrinterModel>().ReverseMap().ForAllMembers(opt => opt.AllowNull());
        CreateMap<Reader, ReaderModel>().ReverseMap().ForAllMembers(opt => opt.AllowNull());
        CreateMap<GPIOConfiguration, GPIOConfigurationDto>().ReverseMap().ForAllMembers(opt => opt.AllowNull());
        CreateMap<ReaderConfiguration, ReaderConfigurationDto>().ReverseMap().ForAllMembers(opt => opt.AllowNull());
        CreateMap<WorkflowSettings, WorkflowSettingsDto>().ReverseMap().ForAllMembers(opt => opt.AllowNull());
    }
}
