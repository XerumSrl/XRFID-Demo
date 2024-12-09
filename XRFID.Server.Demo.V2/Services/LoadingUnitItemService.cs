using AutoMapper;
using Xerum.XFramework.DBAccess.Uow;
using XRFID.Demo.Common.Dto;
using XRFID.Server.Demo.V2.Entities;
using XRFID.Server.Demo.V2.Repositories.Interfaces;

namespace XRFID.Server.Demo.V2.Services;

public class LoadingUnitItemService(ILoadingUnitItemRepository _repository, IMapper _mapper, IUnitOfWork _uowk)
{
    public async Task<List<LoadingUnitItemDto>> GetByLoadingUnitIdAsync(Guid luId)
    {
        List<LoadingUnitItem> result = await _repository.GetAsync(q => q.LoadingUnitId == luId);
        if (!result.Any())
        {
            throw new KeyNotFoundException("Resource not found");
        }
        return _mapper.Map<List<LoadingUnitItemDto>>(result);
    }

    public async Task<LoadingUnitItemDto> CreateAsync(LoadingUnitItemDto loadingUnitDto)
    {
        LoadingUnitItem result = await _repository.CreateAsync(_mapper.Map<LoadingUnitItem>(loadingUnitDto));

        await _uowk.SaveAsync();

        return _mapper.Map<LoadingUnitItemDto>(result);
    }
}
