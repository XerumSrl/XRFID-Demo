using AutoMapper;
using Xerum.XFramework.DBAccess.Uow;
using XRFID.Demo.Common.Dto;
using XRFID.Server.Demo.V2.Entities;
using XRFID.Server.Demo.V2.Repositories.Interfaces;

namespace XRFID.Server.Demo.V2.Services;

public class PrinterService(IPrinterRepository _repository, IMapper _mapper, IUnitOfWork _uowk)
{

    public async Task<List<PrinterDto>> GetAsync()
    {
        List<Printer> result = await _repository.GetAsync();

        if (!result.Any())
        {
            throw new KeyNotFoundException("Resource not found");
        }

        return _mapper.Map<List<PrinterDto>>(result);
    }

    public async Task<PrinterDto> GetByNameAsync(string name)
    {
        List<Printer> resultList = await _repository.GetAsync(q => q.Name == name);
        if (!resultList.Any())
        {
            throw new KeyNotFoundException("Resource not found");
        }

        Printer? result = resultList.FirstOrDefault() ?? throw new KeyNotFoundException("Resource not found");
        return _mapper.Map<PrinterDto>(result);
    }

    public async Task<PrinterDto> GetByIdAsync(Guid id)
    {
        List<Printer> resultList = await _repository.GetAsync(q => q.Id == id);
        if (!resultList.Any())
        {
            throw new KeyNotFoundException("Resource not found");
        }

        Printer? result = resultList.FirstOrDefault() ?? throw new KeyNotFoundException("Resource not found");
        return _mapper.Map<PrinterDto>(result);
    }

    public async Task<PrinterDto> CreateAsync(PrinterDto PrinterDto)
    {
        Printer result = await _repository.CreateAsync(_mapper.Map<Printer>(PrinterDto));

        await _uowk.SaveAsync();

        return _mapper.Map<PrinterDto>(result);
    }

}
