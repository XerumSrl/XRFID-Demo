using Xerum.XFramework.DBAccess.Repositories;
using XRFID.Server.Demo.V2.Database;
using XRFID.Server.Demo.V2.Entities;
using XRFID.Server.Demo.V2.Repositories.Interfaces;

namespace XRFID.Server.Demo.V2.Repositories;

public class RawTagHistoryRepository(XRFIDSampleContext dbContext) : Repository<RawTagHistory>(dbContext), IRawTagHistoryRepository
{
}