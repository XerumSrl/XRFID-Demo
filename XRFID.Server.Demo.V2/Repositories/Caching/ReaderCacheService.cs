using Xerum.XFramework.Common.Enums;
using Xerum.XFramework.DBAccess.Repositories.Caching;
using XRFID.Server.Demo.V2.Entities;

namespace XRFID.Server.Demo.V2.Repositories.Caching;

public class ReaderCacheService(IConfiguration configuration) : CacheService<Reader>(configuration)
{
    public override void Add(Reader reader)
    {
        if (reader.Status == ReaderStatus.Disabled)
        {
            return;
        }
        CacheKey k = new CacheKey { Id = reader.Id };
        cache.TryAdd(k, reader);
    }

    public Reader? GetByName(string name) => cache.Select(s => s.Value).FirstOrDefault(f => f.Name == name);
}

