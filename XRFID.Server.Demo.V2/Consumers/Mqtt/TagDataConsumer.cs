using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Xerum.XFramework.Common.Exceptions;
using Xerum.XFramework.DBAccess.Uow;
using XRFID.Demo.Common.Dto;
using XRFID.Demo.Modules.Mqtt.Contracts;
using XRFID.Server.Demo.V2.DataStores;
using XRFID.Server.Demo.V2.DataStructures.SignalR;
using XRFID.Server.Demo.V2.Entities;
using XRFID.Server.Demo.V2.Hubs;
using XRFID.Server.Demo.V2.Repositories.Interfaces;
using XRFID.Server.Demo.V2.StateMachines.Shipment.Contracts;

namespace XRFID.Server.Demo.V2.Consumers.Mqtt;

public class TagDataConsumer(IReaderRepository _readerRepository,
    ILogger<TagDataConsumer> _logger,
    IRawTagHistoryRepository _rawTagHistoryRepository,
    IHubContext<UiMessageHub> _hubContext,
    IUnitOfWork _uow,
    PointDataStores _pointDataStores) : IConsumer<ZebraTagData>, IConsumer<ZebraDirectionalityTagData>, IConsumer<ZebraRawDirectionalityTagData>
{

    public async Task Consume(ConsumeContext<ZebraTagData> context)
    {
        _logger.LogTrace(context.Message.ToString());
        _logger.LogDebug("[Consume<ZebraTagData>] {HostName}|Epc: {IdHex} Format: {Format}", context.Message.HostName, context.Message.IdHex, context.Message.Format);

        try
        {
            var reader = (await _readerRepository.GetAsync(q => q.Name == context.Message.HostName, ["Movements", "Location"])).FirstOrDefault() ?? throw new Exception("No Reader found");
            var activeMovement = reader?.Movements.FirstOrDefault(a => a.IsActive);

            _logger.LogDebug("[Consume<ZebraTagData>] {HostName}|Getting reader information Id: {Id}", context.Message.HostName, reader.Id);
            _logger.LogDebug("[Consume<ZebraTagData>] {HostName}|starting with EPC {IdHex}", context.Message.HostName, context.Message.IdHex);


            _logger.LogDebug("[Consume<ZebraTagData>] {HostName}|Beginning switch WorkflowType.Shipment with EPC {IdHex}", context.Message.HostName, context.Message.IdHex);

            await context.Publish(new ShipmentTagData
            {
                CorrelationId = reader.CorrelationId, //coincide con l'active movement. In caso di input da client esterni, va gestito mantenuto il correlation id
                ReaderId = reader.Id,
                MovementId = activeMovement.Id /*?? Guid.Empty*/,
                HostName = context.Message.HostName,

                TagAction = new TagDto
                {

                    Epc = context.Message.IdHex.ToUpper(),
                    TagSeenCount = context.Message.Reads ?? 1,
                    PC = context.Message.Pc ?? string.Empty,
                    Tid = context.Message.Tid ?? string.Empty,
                    Rssi = context.Message.PeakRssi ?? 0,

                    ZoneName = reader.Location?.Name ?? "Portal",
                    Timestamp = context.Message.Timestamp

                },
            });
            _logger.LogDebug("[Consume<ZebraTagData>] {HostName}|  with EPC {IdHex}", context.Message.HostName, context.Message.IdHex);


            _logger.LogDebug("[Consume<ZebraTagData>] {HostName}|Responded to XTagData with EPC {IdHex}", context.Message.HostName, context.Message.IdHex);

        }
        catch (Exception ex) when (ex is EnumOutOfRangeException)
        {
            _logger.LogError("[Consume<ZebraTagData>] {Message}", ex.Message);
            return;
        }
        catch (NotImplementedException niex)
        {
            _logger.LogWarning("[Consume<ZebraTagData>] {Message}", niex.Message);
            return;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Consume<ZebraTagData>] Unexpected Exception");
            return;
        }
    }

    public async Task Consume(ConsumeContext<ZebraDirectionalityTagData> context)
    {
        _logger.LogTrace(context.Message.ToString());
        _logger.LogDebug("[Consume<ZebraDirectionalityTagData>] {HostName}|Epc: {IdHex}, ZoneName: {ZoneName}, PrevZoneName: {PrevZoneName}", context.Message.ReaderName, context.Message.IdHex, context.Message.ZoneName, context.Message.PrevZoneName);

        try
        {
            var reader = (await _readerRepository.GetAsyncNoTracking(q => q.Name == context.Message.ReaderName, "Movements")).FirstOrDefault();
            var activeMovement = reader?.Movements.FirstOrDefault(a => a.IsActive);

            _logger.LogDebug("[Consume<ZebraDirectionalityTagData>] {HostName}|Getting reader information Id: {Id}", context.Message.ReaderName, reader?.Id);
            _logger.LogDebug("[Consume<ZebraDirectionalityTagData>] {HostName}|starting with EPC {IdHex}", context.Message.ReaderName, context.Message.IdHex);
            _logger.LogDebug("[Consume<ZebraDirectionalityTagData>] {HostName}|Beginning switch WorkflowType.Shipment with EPC {IdHex}", context.Message.ReaderName, context.Message.IdHex);

            string zoneName = (string.IsNullOrEmpty(context.Message?.ZoneName) ? activeMovement?.Reader?.Location?.Name : context.Message?.ZoneName) ?? "unknown location";

            await context.Publish(new ShipmentTagData
            {
                CorrelationId = reader.CorrelationId, //coincide con l'active movement. In caso di input da client esterni, va gestito mantenuto il correlation id
                ReaderId = reader.Id,
                //MovementId = reader.CorrelationId,
                MovementId = activeMovement.Id /*?? Guid.Empty*/,
                HostName = context.Message.ReaderName,
                TagAction = new TagDto
                {

                    Epc = context.Message.IdHex.ToUpper(),
                    TagSeenCount = 1,

                    PC = string.Empty,
                    Tid = string.Empty,
                    Rssi = 0,

                    Timestamp = context.Message.Timestamp,

                    ZoneName = zoneName,

                    PrevZoneName = context.Message.PrevZoneName ?? string.Empty,

                },
            });
        }
        catch (Exception ex) when (ex is EnumOutOfRangeException)
        {
            _logger.LogError("[Consume<ZebraDirectionalityTagData>] {Message}", ex.Message);
            return;
        }
        catch (NotImplementedException niex)
        {
            _logger.LogWarning("[Consume<ZebraDirectionalityTagData>] {Message}", niex.Message);
            return;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Consume<ZebraDirectionalityTagData>] Unexpected Exception");
            return;
        }
    }

    public async Task Consume(ConsumeContext<ZebraRawDirectionalityTagData> context)
    {
#if DEBUG
        _logger.LogDebug("[Consume<ZebraRawDirectionalityTagData>] Received tag: {0}", context.Message.IdHex);
#else
        _logger.LogTrace("[Consume<RawDirectionalityTagMessage>] Received tag: {0}", context.Message.IdHex);
#endif
        Reader? reader;

        try
        {
            reader = await _readerRepository.GetByNameAsync(context.Message.ReaderName) ?? throw new NullReferenceException("Reader does not exist");

        }
        catch (Exception ex)
        {
#if DEBUG
            _logger.LogDebug(ex, "[Consume<ZebraRawDirectionalityTagData>] Error getting reader from DB");
#else
            _logger.LogTrace(ex, "[Consume<ZebraRawDirectionalityTagData>] Error getting reader from DB");
#endif
            return;
        }

        try
        {

            ZebraRawDirectionalityTagData rawData = context.Message;

            //cacolo i vari x y z
            //double altezzaOggetto = FeetsToMeters(reader.ReaderConfiguration.ModeSpecificSettings?.BasicConfig?.ReaderHeight - reader.ReaderConfiguration.ModeSpecificSettings?.BasicConfig?.TagHeight ?? 0); // Altezza dell'oggetto

            double altezzaOggetto = _pointDataStores.ReaderHeight - _pointDataStores.ObjectHeight;
            double azimuth = rawData.Azimuth; // Azimuth in gradi
            double elevation = rawData.Elevation; // Elevazione in gradi

            // Converti gli angoli in radianti
            double azimuthRadians = azimuth * (Math.PI / 180);
            double elevationRadians = elevation * (Math.PI / 180);

            //proietto sul piano XY la distanza dell'oggetto rispetto a seno diviso coseno (tangente)
            double proiezioneSulPiano = altezzaOggetto * Math.Tan(elevationRadians);

            // Calcola le coordinate cartesiane
            double resultX = proiezioneSulPiano * Math.Sin(azimuthRadians);
            double resultY = proiezioneSulPiano * Math.Cos(azimuthRadians);
            double resultZ = reader.ReaderConfiguration.ModeSpecificSettings?.BasicConfig?.TagHeight ?? 0; // altezzaOggetto; // L'altezza dell'oggetto corrisponde alla coordinata z

            // Calcola la distanza radiale
            double distanceRadians = altezzaOggetto / Math.Tan(elevationRadians);

            RawTagHistory? result = null;

            if (_pointDataStores.dbSave)
            {
                //salvo nella raw data history table
                result = await _rawTagHistoryRepository.CreateAsync(new RawTagHistory
                {
                    Epc = rawData.IdHex,
                    Format = rawData.Format,
                    ReaderName = reader.Name,
                    Azimuth = rawData.Azimuth,
                    AzimuthConfidentiality = rawData.AzimuthConfidentiality,
                    Elevation = rawData.Elevation,
                    ElevationConfidentiality = rawData.ElevationConfidentiality,
                    Timestamp = rawData.Timestamp,

                    Distance = FeetsToMeters(reader.ReaderConfiguration.ModeSpecificSettings?.BasicConfig?.ReaderHeight ?? 0),
                    DistanceCorrection = FeetsToMeters(reader.ReaderConfiguration.ModeSpecificSettings?.BasicConfig?.TagHeight ?? 0),
                    DistanceRadians = distanceRadians,

                    AzimuthRadians = azimuthRadians,
                    ElevationRadians = elevationRadians,

                    ResultX = resultX,
                    ResultY = resultY,
                    ResultZ = resultZ,
                });
            }

            PointMessage? p = null;

            if (rawData.AzimuthConfidentiality == 100 && rawData.ElevationConfidentiality == 100)
            {
                PointMessage point = new PointMessage { X = resultX, Y = resultY, EPC = rawData.IdHex, Timestamp = rawData.Timestamp };
                p = _pointDataStores.AddPoint(point);
            }

            if (p is not null)
            {
                try
                {

                    await _hubContext.Clients.Groups(reader.Name).SendAsync("ReceiveMessage", p);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "");
                }
            }

            await _uow.SaveAsync();
#if DEBUG
            _logger.LogDebug("Create new rawdata for {0} with id {@1}", result?.Epc, result);
#else
            _logger.LogTrace("Create new rawdata for {0} with id {@1}", result?.Epc, result);
#endif

        }
        catch (IndexOutOfRangeException)
        {
#if DEBUG
            _logger.LogError("[Consume<ZebraRawDirectionalityTagData>] Unsupported WorkflowType {a}", reader.WorkflowType);
#else
            _logger.LogTrace("[Consume<ZebraRawDirectionalityTagData>] Unsupported WorkflowType {a}", reader.WorkflowType);
#endif
        }
        catch (EnumOutOfRangeException ex)
        {
#if DEBUG
            _logger.LogError("[Consume<ZebraRawDirectionalityTagData>] {0}", ex.Message);
#else
            _logger.LogTrace("[Consume<ZebraRawDirectionalityTagData>] {0}", ex.Message);
#endif
        }
        catch (Exception ex)
        {
#if DEBUG
            _logger.LogError(ex, "[Consume<ZebraRawDirectionalityTagData>] Unexpected exception");
#else
            _logger.LogTrace(ex, "[Consume<ZebraRawDirectionalityTagData>] Unexpected exception");
#endif
        }
    }

    private static double FeetsToMeters(double feets)
    {
        return feets * 0.3048; // 1 piede equivale a 0.3048 metri
    }
}