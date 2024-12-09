﻿using Xerum.XFramework.DBAccess.Repositories;
using XRFID.Server.Demo.V2.Entities;

namespace XRFID.Server.Demo.V2.Repositories.Interfaces
{
    public interface IReaderRepository : IAuditRepository<Reader>
    {
        Task<Reader?> GetByNameAsync(string name);
    }
}