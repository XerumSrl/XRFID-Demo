using Xerum.XFramework.DBAccess;
using Xerum.XFramework.DBAccess.Repositories;
using XRFID.Server.Demo.V2.Database;
using XRFID.Server.Demo.V2.Entities;
using XRFID.Server.Demo.V2.Repositories.Interfaces;

namespace XRFID.Server.Demo.V2.Repositories;

public class LoadingUnitRepository(IUserService userService, XRFIDSampleContext context) : AuditRepository<LoadingUnit>(userService, context), ILoadingUnitRepository
{
}
