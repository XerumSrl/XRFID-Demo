using Microsoft.EntityFrameworkCore;
using Xerum.XFramework.DBAccess;
using Xerum.XFramework.DBAccess.Repositories;
using XRFID.Server.Demo.V2.Database;
using XRFID.Server.Demo.V2.Entities;
using XRFID.Server.Demo.V2.Repositories.Caching;
using XRFID.Server.Demo.V2.Repositories.Interfaces;

namespace XRFID.Server.Demo.V2.Repositories;

public class ReaderRepository(IUserService userService, XRFIDSampleContext context, ReaderCacheService _cache) : AuditRepository<Reader>(userService, context), IReaderRepository
{
    public async Task<Reader?> GetByNameAsync(string name)
    {
        Reader? reader = _cache.GetByName(name);

        if (reader == null)
        {
            reader = await _table.FirstOrDefaultAsync(x => x.Name == name);
            if (reader != null)
            {
                _cache.Add(reader);
            }
        }

        return reader;
    }
}
