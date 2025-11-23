var builder = DistributedApplication.CreateBuilder(args);

builder.AddContainer("grafana", "grafana/grafana")
       .WithBindMount("../../../compose/grafana/config", "/etc/grafana", isReadOnly: true)
       .WithBindMount("../../../compose/grafana/dashboards", "/var/lib/grafana/dashboards", isReadOnly: true)
       .WithHttpEndpoint(port: 3000, targetPort: 3000, name: "http");

builder.AddContainer("prometheus", "prom/prometheus")
       .WithBindMount("../../../compose/prometheus", "/etc/prometheus", isReadOnly: true)
       .WithHttpEndpoint(port: 9090, targetPort: 9090);

var username = builder.AddParameter("pg-username", "admin");
var password = builder.AddParameter("pg-password", "admin");

var database = builder.AddPostgres("db", username, password, port: 5432)
    .WithPgAdmin()
    .WithDataVolume()
    .AddDatabase("akhbarblazor");  //.AddDatabase("fullstackhero");
// Ahmed Galal
// Note: Ensure that the database name matches the one used in the API project configuration.
// var cache = builder.AddRedis("redis", port: 6379);
var cache = builder.AddGarnet("cache", port: 6379);
    // .WithImage("ghcr.io/microsoft/garnet").WithImageTag("latest");

var api = builder.AddProject<Projects.Server>("webapi")
    // .WithHttpEndpoint(port: 5000)
    // .WithHttpsEndpoint(port: 7000)
    .WithReference(cache)
    .WaitFor(database);

var blazor = builder.AddProject<Projects.Client>("blazor")
    // .WithHttpEndpoint(port: 5100)
    // .WithHttpsEndpoint(port: 7100)
    .WithReference(api);

using var app = builder.Build();

await app.RunAsync();
