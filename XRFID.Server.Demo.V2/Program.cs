using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using MQTTnet.AspNetCore;
using MudBlazor.Services;
using Serilog;
using Syncfusion.Blazor;
using Syncfusion.Licensing;
using System.Reflection;
using System.Text.Json.Serialization;
using Xerum.XFramework.Common;
using Xerum.XFramework.Common.Exceptions;
using Xerum.XFramework.DBAccess;
using Xerum.XFramework.DBAccess.Uow;
using Xerum.XFramework.DefaultLogging;
using XRFID.Demo.Modules.Mqtt;
using XRFID.Demo.Modules.Mqtt.Consumers;
using XRFID.Demo.Modules.Mqtt.Publishers;
using XRFID.Server.Demo.V2.Components;
using XRFID.Server.Demo.V2.Consumers.Frontend;
using XRFID.Server.Demo.V2.Consumers.Mqtt;
using XRFID.Server.Demo.V2.Database;
using XRFID.Server.Demo.V2.DataStores;
using XRFID.Server.Demo.V2.Hubs;
using XRFID.Server.Demo.V2.Mapper;
using XRFID.Server.Demo.V2.Repositories;
using XRFID.Server.Demo.V2.Repositories.Caching;
using XRFID.Server.Demo.V2.Repositories.Interfaces;
using XRFID.Server.Demo.V2.Services;
using XRFID.Server.Demo.V2.StateMachines.Shipment.Consumers;
using XRFID.Server.Demo.V2.StateMachines.Shipment.Contracts;
using XRFID.Server.Demo.V2.StateMachines.Shipment.StateMachines;
using XRFID.Server.Demo.V2.StateMachines.Shipment.States;
using XRFID.Server.Demo.V2.Utilities;
using XRFID.Server.Demo.V2.Workers;

WebApplicationOptions options = new WebApplicationOptions
{
    Args = args,
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default
};

var builder = WebApplication.CreateBuilder(args);
Log.Logger = LogBuilder.CreateStatic(builder.Configuration);

Log.ForContext<Program>().Information("Host starting...");

try
{

    builder.Logging.SetupXerumLogging(Log.Logger);

    builder.Host.UseWindowsService(ws => ws.ServiceName = "XRFID Demo Server");

    // Add services to the container.
    builder.Services.AddRazorComponents().AddInteractiveServerComponents();

    builder.Services.AddSyncfusionBlazor();

    SyncfusionLicenseProvider.RegisterLicense(builder.Configuration["SyncfusionLicense"]);

    builder.Services.AddMudServices();

    builder.Services.AddHttpContextAccessor();

    builder.WebHost.ConfigureKestrel(options =>
    {
        int port = builder.Configuration.GetValue("Mqtt:MqttPort", 1883);
        if (port <= 0)
        {
            port = 1883;
            Log.ForContext<Program>().Warning("Using default MQTT port ({port})", port);
        }
        //todo: port setup using appsettings.json
        options.ListenAnyIP(port, l => l.UseMqtt());
    });

    builder.Services.AddHostedMqttServer(mqttServer => mqttServer.WithoutDefaultEndpoint());
    builder.Services.AddMqttConnectionHandler();
    builder.Services.AddConnections();

    #region ControllerSetup
    builder.Services.AddControllers()
        .ConfigureApiBehaviorOptions(x => x.SuppressMapClientErrors = true)
        .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

    builder.Services.AddSingleton<XResponseDataFactory>();
    #endregion

    builder.Services.AddAutoMapper(m => m.AddProfile<XRFIDSampleProfile>());

    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new NotConfiguredException("Connection string 'DefaultConnection' not found.");


    builder.Services.AddDbContext<XRFIDSampleContext>(options =>
    {
        options.UseSqlServer(connectionString);
        options.UseOpenIddict();
    });


    builder.Services.AddHttpClient();

    builder.Services.AddScoped<IUserService, XUserService>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddSingleton<ReaderCacheService>();
    builder.Services.AddSingleton<PointDataStores>();

    #region Repository
    builder.Services.AddTransient<ILabelRepository, LabelRepository>();
    builder.Services.AddTransient<ILoadingUnitItemRepository, LoadingUnitItemRepository>();
    builder.Services.AddTransient<ILoadingUnitRepository, LoadingUnitRepository>();
    builder.Services.AddTransient<IMovementItemRepository, MovementItemRepository>();
    builder.Services.AddTransient<IMovementRepository, MovementRepository>();
    builder.Services.AddTransient<IPrinterRepository, PrinterRepository>();
    builder.Services.AddTransient<IProductRepository, ProductRepository>();
    builder.Services.AddTransient<IReaderRepository, ReaderRepository>();
    builder.Services.AddTransient<ISkuRepository, SkuRepository>();
    builder.Services.AddTransient<IRawTagHistoryRepository, RawTagHistoryRepository>();

    #endregion

    #region ServiceSetup
    builder.Services.AddTransient<ProductService>();
    builder.Services.AddTransient<LoadingUnitService>();
    builder.Services.AddTransient<ReaderService>();
    builder.Services.AddTransient<LoadingUnitItemService>();
    builder.Services.AddTransient<MovementService>();
    builder.Services.AddTransient<LabelService>();
    builder.Services.AddTransient<PrinterService>();
    builder.Services.AddTransient<WorkflowService>();
    #endregion


    #region API Server
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
            //options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "XRFID Server Demo API", Version = "1.0" });

        string authorizationURIstring = builder.Configuration.GetValue<string>("Auth:Issuer") ?? throw new NotConfiguredException("Auth:Issuer");

        c.AddSecurityDefinition(
            "oauth2",
            new OpenApiSecurityScheme
            {
                Flows = new OpenApiOAuthFlows
                {
                    ClientCredentials = new OpenApiOAuthFlow
                    {
                        Scopes = new Dictionary<string, string>
                        {
                            ["api"] = "api scope"
                        },
                        TokenUrl = new Uri($"{builder.Configuration.GetValue<string>("Auth:Issuer")}connect/token", UriKind.Absolute),
                    },
                },
                In = ParameterLocation.Header,
                Name = HeaderNames.Authorization,
                Type = SecuritySchemeType.OAuth2
            }
        );

        c.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }

                    },
                    new[] { "profile", "role", "email", "api", "admin" }
                }
            }
        );

        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        c.IgnoreObsoleteActions();
        c.IgnoreObsoleteProperties();
        c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

    });
    #endregion

    #region MQTT

    builder.Services.AddZebraManagedMqttClient(m =>
    {
        string? configValue = builder.Configuration.GetValue<string>("Mqtt:MqttClientId");
        if (string.IsNullOrWhiteSpace(configValue))
        {
            configValue = Guid.NewGuid().ToString();
            Log.ForContext<Program>().Information("Using new MqttClientId: ({guid})", configValue);
        }
        m.ClientId = configValue;


        configValue = builder.Configuration.GetValue<string>("Mqtt:MqttServer");
        if (string.IsNullOrWhiteSpace(configValue))
        {
            throw new NotConfiguredException(paramName: "Mqtt:MqttServer");
        }
        m.Server = configValue;


        int port = builder.Configuration.GetValue("Mqtt:MqttPort", 1883);
        if (port <= 0)
        {
            port = 1883;
            Log.ForContext<Program>().Warning("Using default MQTT port ({port})", port);
        }
        m.Port = port;

    });

    builder.Services.AddSingleton(KebabCaseEndpointNameFormatter.Instance);
    #endregion

    builder.Services.AddScoped<CommandUtility>();
    builder.Services.AddScoped<GpoUtility>();

    #region Mass Transit

    builder.Services.AddScoped<IZebraMqttCommandPublisher, ZebraMqttCommandPublisher>();
    builder.Services.AddScoped<IZebraMqttEventPublisher, ZebraMqttEventPublisher>();

    builder.Services.AddMassTransit(mt =>
    {
        //Modules.Mqtt.Consumers
        mt.AddConsumer<ZebraCommandConsumer>();

        //Server.Consumer.Mqtt
        mt.AddConsumer<GpioDataConsumer, GpioDataConsumerDefinition>();
        mt.AddConsumer<HeartbeatConsumer>();
        mt.AddConsumer<MresponseConsumer>();
        mt.AddConsumer<TagDataConsumer, TagDataConsumerDefinition>();

        //Server.Consumers.Frontend;
        mt.AddConsumer<UpdateUiConsumer>();

        #region State Machine
        mt.AddConsumersFromNamespaceContaining<ShipmentGpioConsumer>();
        mt.AddRequestClient<ShipmentGpiData>();
        mt.AddSagaStateMachine<ShipmentStateMachine, ShipmentState, ShipmentStateMachineDefinition>().InMemoryRepository();
        #endregion

        mt.AddDelayedMessageScheduler();

        mt.UsingInMemory((context, cfg) =>
        {
            cfg.ConfigureEndpoints(context);
            cfg.UseDelayedMessageScheduler();
        });
    });
    #endregion

    #region Workers
    builder.Services.AddSingleton<CheckPageWorker>();

    //demo data
    builder.Services.AddHostedService<InitializeWorker>();
    #endregion

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    //app.UseHttpsRedirection();

    app.UseStaticFiles();
    app.UseAntiforgery();

    app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

    app.MapControllers();

    app.MapHub<UiMessageHub>("/uimessagehub");

    await app.RunAsync();

    return 0;
}
catch (HostAbortedException)//EF core migration genaration causes this to be thrown, and it pollutes the logs
{
    return 1;
}
catch (Exception ex)
{
    Log.ForContext<Program>().Fatal(ex, "Host terminated unexpectedly");
    return -1;
}
finally
{
    Log.CloseAndFlush();
}