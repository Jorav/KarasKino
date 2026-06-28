using KarasKino.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()
       .AddLoggerConfigs();

var secretsPath = builder.Configuration["SecretsPath"]
  ?? Path.Combine(builder.Environment.ContentRootPath, "..", "..", "secrets", "local.secrets.json");

if (File.Exists(secretsPath))
{
  builder.Configuration.AddJsonFile(secretsPath, optional: true, reloadOnChange: false);
}

// Railway injects DATABASE_URL - map it to our connection string
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
if (!string.IsNullOrEmpty(databaseUrl))
{
  builder.Configuration["ConnectionStrings:karaskinodb"] = databaseUrl;
}

using var loggerFactory = LoggerFactory.Create(config => config.AddConsole());
var startupLogger = loggerFactory.CreateLogger<Program>();

startupLogger.LogInformation("Starting web host");

builder.Services.AddHealthChecks();

builder.Services.AddCors(options =>
{
  options.AddDefaultPolicy(policy =>
  {
    var allowedOrigins = builder.Configuration["AllowedOrigins"] ?? "*";
    policy.WithOrigins(allowedOrigins.Split(','))
          .AllowAnyHeader()
          .AllowAnyMethod().AllowCredentials();
  });
});

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

var app = builder.Build();

app.UseCors();

await app.UseAppMiddlewareAndSeedDatabase();

app.MapDefaultEndpoints();
app.MapHealthChecks("/health");

app.Run();

public partial class Program { }
