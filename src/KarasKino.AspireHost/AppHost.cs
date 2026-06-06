using System.Net.Sockets;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
  .WithLifetime(ContainerLifetime.Persistent);
  //.WithPgAdmin(); // free pgAdmin UI, optional but useful

var karasKinoDb = postgres.AddDatabase("karaskinodb");

// Papercut SMTP container for email testing
var papercut = builder.AddContainer("papercut", "jijiechen/papercut", "latest")
  .WithEndpoint("smtp", e =>
  {
    e.TargetPort = 25;
    e.Port = 25;
    e.Protocol = ProtocolType.Tcp;
    e.UriScheme = "smtp";
  })
  .WithEndpoint("ui", e =>
  {
    e.TargetPort = 37408;
    e.Port = 37408;
    e.UriScheme = "http";
  });

// Add the web project with the database connection
var api = builder.AddProject<Projects.KarasKino_Api>("api")
  .WithReference(karasKinoDb)
  .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
  .WithEnvironment("Papercut__Smtp__Url", papercut.GetEndpoint("smtp"))
  .WithEnvironment("SecretsPath", Path.Combine(builder.AppHostDirectory, "..", "..", "secrets", "local.secrets.json"))
  .WaitFor(karasKinoDb)
  .WaitFor(papercut);

//builder.AddProject<Projects.KarasKino_WebApp>("frontend");
builder.AddNpmApp("frontend", "../KarasKino.WebApp")
    .WithReference(api)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints();

builder
  .Build()
  .Run();
