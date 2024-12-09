using Xerum.XFramework.DBAccess.Uow;

namespace XRFID.Server.Demo.V2.Database;

public class UnitOfWork(XRFIDSampleContext _context) : UnitOfWork<XRFIDSampleContext>(_context)
{

}
