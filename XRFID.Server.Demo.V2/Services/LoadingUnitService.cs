using AutoMapper;
using Xerum.XFramework.DBAccess.Uow;
using XRFID.Demo.Common.Dto;
using XRFID.Server.Demo.V2.Entities;
using XRFID.Server.Demo.V2.Repositories.Interfaces;

namespace XRFID.Server.Demo.V2.Services;

public class LoadingUnitService(ILoadingUnitRepository _repository, ILoadingUnitItemRepository _itemRepository, IMapper _mapper, IUnitOfWork _uowk)
{
    public async Task<List<LoadingUnitDto>> GetAsync()
    {
        List<LoadingUnit> result = await _repository.GetAsync();
        if (!result.Any())
        {
            throw new KeyNotFoundException("Resource not found");
        }
        return _mapper.Map<List<LoadingUnitDto>>(result);
    }

    public async Task<LoadingUnitDto> GetByReferenceAsync(string reference)
    {
        //TODO check if it's actually called anywhere
        List<LoadingUnit> resultList = await _repository.GetAsync(/*q => q.OrderReference != null && q.OrderReference == reference*/);
        if (!resultList.Any())
        {
            throw new KeyNotFoundException("Resource not found");
        }

        LoadingUnit? result = resultList.FirstOrDefault() ?? throw new KeyNotFoundException("Resource not found");
        return _mapper.Map<LoadingUnitDto>(result);
    }

    public async Task<LoadingUnitDto> GetWithItemsAsync(string reference)
    {
        List<LoadingUnit> resultList = await _repository.GetAsync();

        if (!resultList.Any())
        {
            throw new KeyNotFoundException("Resource not found");
        }

        LoadingUnit? result = resultList.FirstOrDefault() ?? throw new KeyNotFoundException("Resource not found");
        List<LoadingUnitItem> itemResult = await _itemRepository.GetAsync(q => q.LoadingUnitId == result.Id);

        if (result.LoadingUnitItems is null)
        {
            result.LoadingUnitItems = new List<LoadingUnitItem>();
        }
        result.LoadingUnitItems = itemResult;

        return _mapper.Map<LoadingUnitDto>(result);
    }

    public async Task<LoadingUnitDto> CreateAsync(LoadingUnitDto loadingUnitDto)
    {
        LoadingUnit result = await _repository.CreateAsync(_mapper.Map<LoadingUnit>(loadingUnitDto));

        await _uowk.SaveAsync();

        return _mapper.Map<LoadingUnitDto>(result);
    }
}
