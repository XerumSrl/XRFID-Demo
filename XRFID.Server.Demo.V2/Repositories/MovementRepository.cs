using Xerum.XFramework.DBAccess;
using Xerum.XFramework.DBAccess.Repositories;
using XRFID.Server.Demo.V2.Database;
using XRFID.Server.Demo.V2.Entities;
using XRFID.Server.Demo.V2.Repositories.Interfaces;

namespace XRFID.Server.Demo.V2.Repositories;

public class MovementRepository(IUserService userService, XRFIDSampleContext context) : AuditRepository<Movement>(userService, context), IMovementRepository
{
    public async Task<int> DeactivateByReaderId(Guid readerId)
    {
        return await ExecuteUpdateAsync(w => w.ReaderId == readerId, e => e.SetProperty(p => p.IsActive, false));
    }

    //TODO check again
    //public async Task<Movement> UpdateStatusAsync(Guid movementId)
    //{
    //    Movement movWithItems = await _table.Where(w => w.Id == movementId)
    //                                        .Include("MovementItems")
    //                                        .FirstOrDefaultAsync() ?? throw new KeyNotFoundException($"Movement with id {movementId} NOT found");

    //    IEnumerable<MovementItem> rowsWithErrors = movWithItems.MovementItems.Where(q => q.Status == ItemStatus.NotFound || q.Status == ItemStatus.Unexpected || q.Status == ItemStatus.Overflow);

    //    if (!rowsWithErrors.Any())
    //    {
    //        movWithItems.MissingItem = false;
    //        movWithItems.UnexpectedItem = false;
    //        movWithItems.OverflowItem = false;
    //        movWithItems.IsValid = true;
    //    }
    //    else
    //    {
    //        if (rowsWithErrors.Where(x => x.Status == ItemStatus.NotFound).Any())
    //        {
    //            movWithItems.MissingItem = true;
    //        }
    //        else
    //        {
    //            movWithItems.MissingItem = false;
    //        }
    //        if (rowsWithErrors.Where(x => x.Status == ItemStatus.Unexpected).Any())
    //        {
    //            movWithItems.UnexpectedItem = true;
    //        }
    //        else
    //        {
    //            movWithItems.UnexpectedItem = false;
    //        }
    //        if (rowsWithErrors.Where(x => x.Status == ItemStatus.Overflow).Any())
    //        {
    //            movWithItems.OverflowItem = true;
    //        }
    //        else
    //        {
    //            movWithItems.OverflowItem = false;
    //        }
    //        movWithItems.IsValid = false;
    //    }

    //    movWithItems.IsConsolidated = true;

    //    return await Update(movWithItems);
    //}
}
