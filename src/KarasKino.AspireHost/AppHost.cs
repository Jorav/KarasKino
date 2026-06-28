using System.Net.Sockets;
using Microsoft.Extensions.Configuration;

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

var frontendPort = builder.Configuration.GetValue<int>("Ports:Frontend");
var apiPort = builder.Configuration.GetValue<int>("Ports:Api");

var frontend = builder.AddNpmApp("frontend", "../KarasKino.WebApp")
    .WithHttpEndpoint(port: frontendPort, env: "PORT")
    .WithExternalHttpEndpoints();

// Add the web project with the database connection
var api = builder.AddProject<Projects.KarasKino_Api>("api")
  .WithReference(karasKinoDb)
  .WithEnvironment("FrontendUrl", frontend.GetEndpoint("http"))
  .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
  .WithEnvironment("Papercut__Smtp__Url", papercut.GetEndpoint("smtp"))
  .WithEnvironment("SecretsPath", Path.Combine(builder.AppHostDirectory, "..", "..", "secrets", "local.secrets.json"))
  .WaitFor(karasKinoDb)
  .WaitFor(papercut);

frontend.WithReference(api);

builder
  .Build()
  .Run();
