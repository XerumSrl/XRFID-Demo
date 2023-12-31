﻿using Microsoft.IdentityModel.Tokens;
using Xerum.XFramework.Common.Enums;
using XRFID.Demo.Common.Enumerations;
using XRFID.Demo.Server.Entities;
using XRFID.Demo.Server.Repositories;
using XRFID.Demo.Server.ViewModels;
using XRFID.Demo.Server.ViewModels.Enums;

namespace XRFID.Demo.Server.Workers;

/*@@ TO DO @@
 * split for reader ID
 */

public class CheckPageWorker
{
    private readonly MovementItemRepository movementItemRepository;
    private readonly MovementRepository movementRepository;
    private readonly ILogger<CheckPageWorker> logger;


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
    public CheckPageWorker(IServiceProvider serviceProvider, ILogger<CheckPageWorker> logger)
    {
        var scope = serviceProvider.CreateScope();
        movementItemRepository = scope.ServiceProvider.GetRequiredService<MovementItemRepository>();
        movementRepository = scope.ServiceProvider.GetRequiredService<MovementRepository>();

        this.logger = logger;
    }

    public async Task SetViewItem(string epc)
    {
        CheckItemModel? foundItem = _viewItems.Where(w => w.Epc.ToUpper() == epc.ToUpper()).FirstOrDefault();
        if (foundItem is not null)
        {
            if (foundItem.CheckStatus == CheckStatusEnum.NotFound)
            {
                foundItem.CheckStatus = CheckStatusEnum.Found;
            }

            foundItem.Direction = _movement?.Direction ?? MovementDirection.In;
        }
        else
        {
            var item = (await movementItemRepository.GetAsync(q => q.Epc.ToUpper() == epc.ToUpper())).FirstOrDefault();
            if (item is null)
            {
                return;
            }

            _viewItems.Add(new CheckItemModel
            {
                Name = item.Name ?? string.Empty,
                Epc = epc,
                Description = item.Description,
                CheckStatus = item.Status == ItemStatus.Found ? CheckStatusEnum.Found :
                                         (item.Status == ItemStatus.NotFound ? CheckStatusEnum.NotFound : CheckStatusEnum.Error),
                DateTime = item.LastModificationTime,
                Direction = _movement?.Direction ?? MovementDirection.In,
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

        _movement = await movementRepository.GetAsync(Id);

        List<MovementItem> itemList = await movementItemRepository.GetAsync(q => q.MovementId == Id);
        if (itemList.IsNullOrEmpty())
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
                                         (item.Status == ItemStatus.NotFound ? CheckStatusEnum.NotFound : CheckStatusEnum.Error),
                    DateTime = item.LastModificationTime,
                    Direction = _movement?.Direction ?? Common.Enumerations.MovementDirection.In,
                };
                _viewItems.Add(vItem);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "");
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
