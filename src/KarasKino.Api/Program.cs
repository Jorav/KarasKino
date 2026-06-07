using JasperFx.Resources;
using KarasKino.Api.Configurations;
using Wolverine;
using Wolverine.Postgresql;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()
       .AddLoggerConfigs();

var secretsPath = builder.Configuration["SecretsPath"];
if (!string.IsNullOrEmpty(secretsPath))
{
  builder.Configuration.AddJsonFile(secretsPath, optional: true, reloadOnChange: false);
}

using var loggerFactory = LoggerFactory.Create(config => config.AddConsole());
var startupLogger = loggerFactory.CreateLogger<Program>();

startupLogger.LogInformation("Starting web host");

builder.Services.AddOptionConfigs(builder.Configuration, startupLogger, builder);
builder.Services.AddServiceConfigs(startupLogger, builder);

builder.Services.AddFastEndpoints()
                .SwaggerDocument(o =>
                {
                  o.DocumentSettings = s =>
                  {
                    s.Title = "Clean Architecture API";
                    s.Version = "v1";
                    s.Description = "HTTP endpoints for the Clean Architecture sample application.";
                  };
                  o.ShortSchemaNames = true;
                });

builder.Host.UseWolverine(opts =>
{
  opts.CodeGeneration.TypeLoadMode = builder.Environment.IsDevelopment()
        ? JasperFx.CodeGeneration.TypeLoadMode.Auto
        : JasperFx.CodeGeneration.TypeLoadMode.Static;

  var connectionString = builder.Configuration.GetConnectionString("karaskinodb")
                         ?? builder.Configuration.GetConnectionString("DefaultConnection");

  opts.UsePostgresqlPersistenceAndTransport(connectionString!, "wolverine").AutoProvision();

  opts.Policies.AutoApplyTransactions();
  opts.Policies.UseDurableLocalQueues();

  opts.Discovery.IncludeAssembly(typeof(KarasKino.Application.Movies.FindByImdbId.FindByImdbIdHandler).Assembly);
});
builder.Services.AddResourceSetupOnStartup();

var app = builder.Build();

await app.UseAppMiddlewareAndSeedDatabase();

app.MapDefaultEndpoints();

app.Run();

public partial class Program { }
