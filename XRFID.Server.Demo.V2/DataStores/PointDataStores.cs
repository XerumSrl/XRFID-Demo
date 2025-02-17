using XRFID.Server.Demo.V2.DataStructures.SignalR;

namespace XRFID.Server.Demo.V2.DataStores;

public class PointDataStores
{
    private List<List<PointMessage>> dataMessages = [];
    public float ReaderHeight { get; set; } = 5;
    public float ObjectHeight { get; set; } = 1;
    public string EpcFilter { get; set; } = "51AF";
    public bool dbSave = false;

    public PointMessage? AddPoint(PointMessage newPoint)
    {
        lock (dataMessages)
        {
            List<PointMessage> points = [];
            bool found = false;
            if (!dataMessages.Any())
            {
                dataMessages.Add([newPoint]);
                return null;
            }
            else
            {
                foreach (List<PointMessage> list in dataMessages)
                {
                    if (list[0].EPC == newPoint.EPC)
                    {
                        list.Add(newPoint);
                        found = true;
                        points = list;
                        break;
                    }
                }

                if (!found)
                {

                    dataMessages.Add([newPoint]);
                    return null;
                }
                else
                {
                    DateTimeOffset initTimestamp = points.Min(p => p.Timestamp);

                    if (newPoint.Timestamp < initTimestamp + TimeSpan.FromMilliseconds(500))
                    {
                        points.Add(newPoint);
                    }
                    else
                    {
                        double x = 0;
                        double y = 0;
                        foreach (PointMessage point in points)
                        {
                            x += point.X;
                            y += point.Y;
                        }

                        PointMessage p = new PointMessage
                        {
                            X = x / points.Count,
                            Y = y / points.Count,
                            EPC = newPoint.EPC,
                            Timestamp = newPoint.Timestamp
                        };

                        p.Timestamp = p.Timestamp.ToOffset(TimeSpan.FromHours(1));

                        points.Clear();

                        points.Add(newPoint);

                        return p;
                    }
                }

                return null;
            }

        }
    }

}

