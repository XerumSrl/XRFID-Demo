using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using XRFID.Server.Demo.V2.DataStructures.SignalR;
using XRFID.Server.Demo.V2.Entities;

namespace XRFID.Server.Demo.V2.Components.Locationing;

public partial class PlotTrackHistory : ComponentBase
{
    private void FetchData(Movement movement)
    {
        var movementItems = _movementItemRepo.AsQueryable().AsNoTracking().Where(p => p.Epc == epc && p.MovementId == movement.Id).OrderByDescending(p => p.DateCreated);

        ZoneHistories = [];

        foreach (MovementItem movementItem in movementItems)
        {
            if (movementItem.ZoneName != string.Empty || movementItem.PreviousZoneName != string.Empty)
            {
                ZoneHistories.Add(new ZoneMovement
                {
                    dateTime = movementItem.LastRead,
                    PrevZone = movementItem.PreviousZoneName ?? string.Empty,
                    Zone = movementItem.ZoneName
                });
            }
        }

        RawTagHistory[] pts;

        if (movementItems.Count() > 1)
        {
            pts = _tagHistoryRepo.AsQueryable().AsNoTracking().Where(t => t.ReaderName == reader && t.Epc == epc && t.Timestamp >= movementItems.Last().DateCreated && t.Timestamp <= movementItems.First().DateCreated).OrderByDescending(p => p.Timestamp).ToArray();

        }
        else
        {
            pts = _tagHistoryRepo.AsQueryable().AsNoTracking().Where(t => t.ReaderName == reader && t.Epc == epc && t.Timestamp >= movementItems.Last().DateCreated && t.Timestamp <= movementItems.Last().LastRead).OrderByDescending(p => p.Timestamp).ToArray();

        }

        Points = [];
        PointMessageX = [];
        PointMessageY = [];

        foreach (RawTagHistory pointData in pts)
        {
            if (pointData.AzimuthConfidentiality == 100 && pointData.ElevationConfidentiality == 100)
            {
                Points.Add(new PointMessage { X = pointData.ResultX, Y = pointData.ResultY, EPC = pointData.Epc, Timestamp = pointData.Timestamp });
                PointMessageX.Add(new ChartData { Timestamp = pointData.Timestamp, Y = pointData.ResultX });
                PointMessageY.Add(new ChartData { Timestamp = pointData.Timestamp, Y = pointData.ResultY });
            }
        }

        int[,] matrix = new int[13, 13];

        // Lista di punti
        foreach (RawTagHistory point in pts)
        {
            // Calcola l'indice della matrice corrispondente al punto
            int x = (int)Math.Round(point.ResultX) + 6;
            int y = (int)Math.Round(point.ResultY) + 6;

            // Controlla se l'indice è all'interno della matrice
            if (x >= 0 && x < 13 && y >= 0 && y < 13)
            {
                // Incrementa il valore della cella corrispondente
                matrix[x, y]++;
            }
        }

        HeatMapData = matrix;

    }

    private void CheckMovementAsync()
    {
        //Movement activeMovement = movementRepo.AsQueryable().AsNoTracking().Where(p => p.IsActive == true).FirstOrDefault();

        var movments = _movementItemRepo.AsQueryable().AsNoTracking().Where(p => p.Epc == epc).Select(s => s.Movement).Distinct().OrderByDescending(p => p.DateCreated);

        MovementList = [];

        foreach (var movment in movments)
        {
            MovementList.Add(movment);
        }
    }


    public void Dispose()
    {

    }
}
