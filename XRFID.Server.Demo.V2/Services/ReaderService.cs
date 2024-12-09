using AutoMapper;
using Xerum.XFramework.DBAccess.Uow;
using XRFID.Demo.Common.Dto;
using XRFID.Demo.Common.Dto.Create;
using XRFID.Server.Demo.V2.Entities;
using XRFID.Server.Demo.V2.Entities.GPIOCfg;
using XRFID.Server.Demo.V2.Entities.ReaderCfg;
using XRFID.Server.Demo.V2.Repositories.Interfaces;

namespace XRFID.Server.Demo.V2.Services;

public class ReaderService(IReaderRepository _repository, IMapper _mapper, IUnitOfWork _uowk)
{
    public async Task<ReaderDto> GetByNameAsync(string name)
    {
        List<Reader> resultList = await _repository.GetAsync(q => q.Name == name);
        if (!resultList.Any())
        {
            throw new KeyNotFoundException("Resource not found");
        }

        Reader? result = resultList.FirstOrDefault() ?? throw new KeyNotFoundException("Resource not found");
        return _mapper.Map<ReaderDto>(result);
    }

    public async Task<ReaderDto> CreateAsync(ReaderDto readerDto)
    {
        Reader result = await _repository.CreateAsync(_mapper.Map<Reader>(readerDto));

        await _uowk.SaveAsync();

        return _mapper.Map<ReaderDto>(result);
    }

    public async Task<ReaderDto> CreateAsync(MinimalReaderCreateDto readerDto)
    {
        Reader result = await _repository.CreateAsync(new Reader
        {
            Id = readerDto.Id,
            Name = readerDto.Name,
            Reference = readerDto.Name,
            GPIOConfiguration = new GPIOConfiguration(),
            ReaderConfiguration = new ReaderConfiguration
            {
                WorkflowSettings = new WorkflowSettings(),
                ModeSpecificSettings = new ModeSpecificSettings()
            }

        });
        await _uowk.SaveAsync();
        return _mapper.Map<ReaderDto>(result);
    }
}
