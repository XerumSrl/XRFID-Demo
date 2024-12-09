using AutoMapper;
using Xerum.XFramework.DBAccess.Uow;
using XRFID.Demo.Common.Dto;
using XRFID.Server.Demo.V2.Entities;
using XRFID.Server.Demo.V2.Repositories.Interfaces;

namespace XRFID.Server.Demo.V2.Services;

public class MovementService(IMovementRepository _repository, IMapper _mapper, IUnitOfWork _uowk)
{

    public async Task<MovementDto> CreateAsync(MovementDto movementDto)
    {
        Movement result = await _repository.CreateAsync(_mapper.Map<Movement>(movementDto));

        await _uowk.SaveAsync();

        return _mapper.Map<MovementDto>(result);
    }
}
