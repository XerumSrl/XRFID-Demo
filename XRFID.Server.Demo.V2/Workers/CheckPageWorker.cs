using Xerum.XFramework.Common.Enums;
using XRFID.Demo.Common.Enumerations;
using XRFID.Server.Demo.V2.Entities;
using XRFID.Server.Demo.V2.Repositories.Interfaces;
using XRFID.Server.Demo.V2.ViewModels;
using XRFID.Server.Demo.V2.ViewModels.Enums;

namespace XRFID.Server.Demo.V2.Workers;

/*@@ TO DO @@
 * split for reader ID
 */

public class CheckPageWorker(IServiceProvider _serviceProvider, ILogger<CheckPageWorker> _logger)
{

    private readonly IMovementItemRepository movementItemRepository;
    private readonly IMovementRepository movementRepository;



    private List<CheckItemModel> _viewItems = new List<CheckItemModel>();
    private int _itemCount = 0;
    private Guid _activeId = Guid.Empty;

    private Movement _movement;

    #region GPI
    public bool Gpi1IsOn = false;
    public bool Gpi2IsOn = false;
    public bool Gpi3IsOn = false;
    #endregion

    #region States
    private StateMachineStateEnum _stateMachineState = StateMachineStateEnum.Stop;
    #endregion

    public async Task SetViewItem(string epc)
    {

        CheckItemModel? foundItem = _viewItems.Where(w => w.Epc.ToUpper() == epc.ToUpper()).FirstOrDefault();

        await using AsyncServiceScope scope = _serviceProvider.CreateAsyncScope();

        MovementItem? mi = (await scope.ServiceProvider.GetRequiredService<IMovementItemRepository>().GetAsync(i => i.Epc.ToUpper() == epc.ToUpper())).OrderByDescending(i => i.DateCreated).FirstOrDefault();

        if (foundItem is not null)
        {

            if (foundItem.CheckStatus == CheckStatusEnum.NotFound)
            {
                foundItem.CheckStatus = CheckStatusEnum.Found;

            }

            foundItem.PrevZoneName = mi.PreviousZoneName ?? string.Empty;
            foundItem.ZoneName = mi.ZoneName;
            foundItem.Direction = _movement?.Direction ?? MovementDirection.In;
        }
        else
        {

            if (mi is null)
            {
                return;
            }

            _viewItems.Add(new CheckItemModel
            {
                Name = mi.Name ?? string.Empty,
                Epc = epc,
                Description = mi.Description,
                CheckStatus = mi.Status == ItemStatus.Found ? CheckStatusEnum.Found :
                                         mi.Status == ItemStatus.NotFound ? CheckStatusEnum.NotFound : CheckStatusEnum.Error,
                DateTime = mi.DateUpdated.DateTime,
                Direction = _movement?.Direction ?? MovementDirection.In,
                ZoneName = mi.ZoneName,
                PrevZoneName = mi.PreviousZoneName ?? string.Empty
            });
        }

    }

    public List<CheckItemModel> GetViewItems()
    {
        List<CheckItemModel> items = new List<CheckItemModel>();
        items = _viewItems.OrderByDescending(o => o.DateTime).ToList();

        return items;
    }

    public async Task<bool> IdIsEqual(Guid Id)
    {

        if (_activeId == Id && _activeId != Guid.Empty)
        {
            return true;
        }

        _activeId = Id;
        _viewItems.Clear();

        await using AsyncServiceScope scope = _serviceProvider.CreateAsyncScope();

        _movement = await scope.ServiceProvider.GetRequiredService<IMovementRepository>().GetAsync(Id);

        List<MovementItem> itemList;

        itemList = await scope.ServiceProvider.GetRequiredService<IMovementItemRepository>().GetAsyncNoTracking(q => q.MovementId == _movement.Id);


        if (!itemList.Any())
        {
            throw new Exception("No items");
        }

        _itemCount = itemList.Count;

        foreach (var item in itemList)
        {
            try
            {
                CheckItemModel vItem = new CheckItemModel
                {
                    Name = item.Name ?? string.Empty,
                    Epc = item.Epc,
                    Description = item.Description,
                    CheckStatus = item.Status == ItemStatus.Found ? CheckStatusEnum.Found :
                                         item.Status == ItemStatus.NotFound ? CheckStatusEnum.NotFound : CheckStatusEnum.Error,
                    DateTime = item.DateUpdated.DateTime,
                    Direction = _movement?.Direction ?? MovementDirection.In,
                    ZoneName = item.ZoneName,
                    PrevZoneName = item.PreviousZoneName ?? string.Empty
                };
                _viewItems.Add(vItem);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "");
            }
        }

        return false;
    }

    public async Task<bool> StartStop(int pin)
    {
        switch (pin)
        {
            case 1:
                Gpi1IsOn = !Gpi1IsOn;
                return Gpi1IsOn;
            case 2:
                Gpi2IsOn = !Gpi2IsOn;
                return Gpi2IsOn;
            case 3:
                Gpi3IsOn = !Gpi3IsOn;
                return Gpi3IsOn;
            default:
                Gpi1IsOn = false;
                Gpi2IsOn = false;
                Gpi3IsOn = false;
                return false;
        }
    }

    public async Task EditSMStatus(StateMachineStateEnum status)
    {
        _stateMachineState = status;
    }

    public StateMachineStateEnum GetSMStatus()
    {
        return _stateMachineState;
    }

    public bool ItemsIsEmpty()
    {
        return !_viewItems.Any();
    }

    public void Reset()
    {
        int remainingItems = _itemCount;
        List<CheckItemModel> items = new List<CheckItemModel>(_itemCount);
        foreach (CheckItemModel item in _viewItems)
        {
            if (item.CheckStatus != CheckStatusEnum.Error)
            {
                item.CheckStatus = CheckStatusEnum.NotFound;
                item.PrevZoneName = string.Empty;
                item.ZoneName = string.Empty;
                items.Add(item);
                remainingItems--;
                if (remainingItems == 0)//if i already found all the items from my lu I jump out of the loop
                {
                    break;
                }
            }
        }
        _viewItems = items;
    }
}
