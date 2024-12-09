using AutoMapper;
using Xerum.XFramework.DBAccess.Uow;
using XRFID.Demo.Common.Dto;
using XRFID.Server.Demo.V2.Entities;
using XRFID.Server.Demo.V2.Repositories.Interfaces;

namespace XRFID.Server.Demo.V2.Services;

public class ProductService(IProductRepository _repository, IMapper _mapper, IUnitOfWork _uowk)
{
    public async Task<List<ProductDto>> GetAsync(string term)
    {
        List<Product> result = await _repository.GetAsync(q => q.Name != null && q.Name.Contains(term) || q.SerialNumber != null && q.SerialNumber.Contains(term));
        if (!result.Any())
        {
            throw new KeyNotFoundException("Resource not found");
        }
        return _mapper.Map<List<ProductDto>>(result);
    }

    public async Task<ProductDto> GetByEpcAsync(string epc)
    {
        List<Product> resultList = await _repository.GetAsync(q => q.Epc == epc);
        if (!resultList.Any())
        {
            throw new KeyNotFoundException("Resource not found");
        }

        Product? result = resultList.FirstOrDefault() ?? throw new KeyNotFoundException("Resource not found");
        return _mapper.Map<ProductDto>(result);
    }

    public async Task<ProductDto> CreateAsync(ProductDto loadingUnitDto)
    {
        Product result = await _repository.CreateAsync(_mapper.Map<Product>(loadingUnitDto));

        await _uowk.SaveAsync();

        return _mapper.Map<ProductDto>(result);
    }

    public async Task<ProductDto?> GetByCodeAsync(string code)
    {
        List<Product> resultList = await _repository.GetAsync(q => q.SerialNumber == code);
        if (!resultList.Any())
        {
            throw new KeyNotFoundException("Resource not found");
        }

        Product? result = resultList.FirstOrDefault() ?? throw new KeyNotFoundException("Resource not found");
        return _mapper.Map<ProductDto>(result);
    }
}
