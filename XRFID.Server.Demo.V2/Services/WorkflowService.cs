using AutoMapper;
using Xerum.XFramework.DBAccess.Uow;
using XRFID.Demo.Common.Dto;
using XRFID.Server.Demo.V2.Entities;
using XRFID.Server.Demo.V2.Repositories.Interfaces;

namespace XRFID.Server.Demo.V2.Services;

public class WorkflowService(ILoadingUnitRepository _loadingUnitRepository
    , IReaderRepository _readerRepository
    , ISkuRepository _skuRepository
    , ILogger<WorkflowService> _logger
    , IUnitOfWork _uow
    , IMapper _mapper)
{

    public async Task<LoadingUnitDto> GenerateLoadingUnitFromEpcs(IEnumerable<string> epcs, string readerName)
    {
        try
        {
            var reader = (await _readerRepository.GetAsync(g => g.Name == readerName)).FirstOrDefault();

            if (reader is null)
            {
                throw new Exception("Reader not found");
            }

            if (epcs is null || !epcs.Any())
            {
                throw new Exception("No Epcs found");
            }

            Sku newSku = new Sku
            {
                Name = "Sku_" + DateTimeOffset.Now,
                Reference = "Sku_" + DateTimeOffset.Now,
                DateCreated = DateTimeOffset.Now,
                Products = []
            };

            LoadingUnit lu = new LoadingUnit
            {
                Name = readerName + DateTimeOffset.Now,
                Reference = readerName + DateTimeOffset.Now,
                ReaderId = reader.Id,
                LoadingUnitItems = []
            };

            foreach (var epc in epcs)
            {
                newSku.Products.Add(new Product
                {
                    Name = epc,
                    Reference = epc + DateTimeOffset.Now,
                    Epc = epc,
                    SerialNumber = epc.Substring(epc.Length - 10),
                    DateCreated = DateTimeOffset.Now

                });

                lu.LoadingUnitItems.Add(new LoadingUnitItem
                {
                    Name = epc + DateTimeOffset.Now,
                    Reference = epc + DateTimeOffset.Now,
                    Epc = epc,
                    DateCreated = DateTimeOffset.Now,
                });
            }

            await _skuRepository.CreateAsync(newSku);

            LoadingUnit newLu = await _loadingUnitRepository.CreateAsync(lu);

            await _uow.SaveAsync();

            return _mapper.Map<LoadingUnitDto>(newLu);


        }
        catch (Exception e)
        {

            _logger.LogError(e, "");
            throw;
        }

    }
}

