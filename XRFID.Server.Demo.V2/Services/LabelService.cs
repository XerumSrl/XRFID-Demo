using AutoMapper;
using Xerum.XFramework.DBAccess.Uow;
using XRFID.Demo.Common.Dto;
using XRFID.Server.Demo.V2.Entities;
using XRFID.Server.Demo.V2.Repositories.Interfaces;

namespace XRFID.Server.Demo.V2.Services;

public class LabelService(ILabelRepository _repository, IMapper _mapper, IUnitOfWork _uowk)
{

    public async Task<List<LabelDto>> GetAsync()
    {
        List<Label> labels = await _repository.GetAsync();

        if (!labels.Any())
        {
            throw new KeyNotFoundException("Resource not found");
        }

        return _mapper.Map<List<LabelDto>>(labels);
    }

    public async Task<LabelDto> GetByIdAsync(Guid id)
    {
        List<Label> resultList = await _repository.GetAsync(q => q.Id == id);
        if (!resultList.Any())
        {
            throw new KeyNotFoundException("Resource not found");
        }

        Label? result = resultList.FirstOrDefault() ?? throw new KeyNotFoundException("Resource not found");
        return _mapper.Map<LabelDto>(result);
    }

    public async Task<LabelDto> GetByNameAsync(string name)
    {
        List<Label> resultList = await _repository.GetAsync(q => q.Name == name);
        if (!resultList.Any())
        {
            throw new KeyNotFoundException("Resource not found");
        }

        Label? result = resultList.OrderByDescending(o => o.Version).FirstOrDefault() ?? throw new KeyNotFoundException("Resource not found");
        return _mapper.Map<LabelDto>(result);
    }

    public async Task<LabelDto> CreateAsync(LabelDto LabelDto)
    {
        Label result = await _repository.CreateAsync(_mapper.Map<Label>(LabelDto));

        await _uowk.SaveAsync();

        return _mapper.Map<LabelDto>(result);
    }
}
