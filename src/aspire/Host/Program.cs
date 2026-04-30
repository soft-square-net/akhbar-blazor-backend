using static System.Net.WebRequestMethods;

var builder = DistributedApplication.CreateBuilder(args);

//builder.AddContainer("grafana", "grafana/grafana")
//       .WithBindMount("../../../compose/grafana/config", "/etc/grafana", isReadOnly: true)
//       .WithBindMount("../../../compose/grafana/dashboards", "/var/lib/grafana/dashboards", isReadOnly: true)
//       .WithHttpEndpoint(port: 3000, targetPort: 3000, name: "http");

//builder.AddContainer("prometheus", "prom/prometheus")
//       .WithBindMount("../../../compose/prometheus", "/etc/prometheus", isReadOnly: true)
//       .WithHttpEndpoint(port: 9090, targetPort: 9090);

var username = builder.AddParameter("pg-username", "admin");
var password = builder.AddParameter("pg-password", "admin");

var database = builder.AddPostgres("db", username, password, port: 5432)
    .WithPgAdmin()  /******** * Uncomment to include pgAdmin for database management * ********/
    .WithDataVolume()
    .AddDatabase("akhbarblazor");  //.AddDatabase("fullstackhero");
// Ahmed Galal
// Note: Ensure that the database name matches the one used in the API project configuration.
// var cache = builder.AddRedis("redis", port: 6379);
var cache = builder.AddGarnet("cache", port: 6379);
// .WithImage("ghcr.io/microsoft/garnet").WithImageTag("latest");

////////////////////////////////////////////////////////////////////////////////////////////////////
//                                          BLAZOR WASM HOST
////////////////////////////////////////////////////////////////////////////////////////////////////

var blazor = builder.AddProject<Projects.Client>("blazor")
    // .WithHttpEndpoint(port: 5100)
    // .WithHttpsEndpoint(port: 7100)

    .WithEndpoint(endpointName: "https", callback: static endpoint =>
    {
        // Sets the actual port the Blazor WASM app runs on
        endpoint.TargetPort = 7100;
        // Sets the port for the Aspire proxy (optional, can be same as TargetPort)
        endpoint.Port = 7300;
    }); // Use your desired fixed port;

////////////////////////////////////////////////////////////////////////////////////////////////////
//                                          WEB API HOST
////////////////////////////////////////////////////////////////////////////////////////////////////


var api = builder.AddProject<Projects.Server>("webapi")
    // .WithHttpEndpoint(port: 5000)
    // .WithHttpsEndpoint(port: 7000)
    .WithReference(cache)
    .WithReference(blazor)
    .WaitFor(database);

blazor.WaitFor(api)
    .WithReference(api);

////////////////////////////////////////////////////////////////////////////////////////////////////
//                                          ELSA STUDIO
////////////////////////////////////////////////////////////////////////////////////////////////////


//var elsaStudio = builder.AddProject<Projects.ElsaStudioBlazorWasm>("elsaStudio")
//    .WithEndpoint(endpointName: "https", callback: static endpoint =>
//    {
//        endpoint.TargetPort = 7250;
//        endpoint.Port = 7350;
//    });

////////////////////////////////////////////////////////////////////////////////////////////////////
//                              MINIO / S3 COMPATIBLE OBJECT STORAGE
////////////////////////////////////////////////////////////////////////////////////////////////////
//// Object storage (MinIO, S3-compatible)
//const string MinioBucket = "fsh-uploads";
//var minioUser = builder.AddParameter("minio-user", "minioadmin");
//var minioPassword = builder.AddParameter("minio-password", "minioadmin", secret: true);

//var minio = builder.AddContainer("minio", "minio/minio")
//    .WithArgs("server", "/data", "--console-address", ":9001")
//    .WithHttpEndpoint(port: 9000, targetPort: 9000, name: "minoioapi")
//    .WithHttpEndpoint(port: 9001, targetPort: 9001, name: "console")
//    .WithEnvironment("MINIO_ROOT_USER", minioUser)
//    .WithEnvironment("MINIO_ROOT_PASSWORD", minioPassword)
//    .WithVolume("fsh-minio-data", "/data")
//    .WithLifetime(ContainerLifetime.Persistent);

//var minioInitScript = $$"""
//        until mc alias set local http://minio:9000 "$MC_USER" "$MC_PASS"; do
//            echo "waiting for minio...";
//            sleep 2;
//        done;
//        mc mb --ignore-existing local/{{MinioBucket}};
//        mc anonymous set download local/{{MinioBucket}};
//    """;

//var minioInit = builder.AddContainer("minio-init", "minio/mc")
//    .WithEntrypoint("/bin/sh")
//    .WithArgs("-c", minioInitScript)
//    .WithEnvironment("MC_USER", minioUser)
//    .WithEnvironment("MC_PASS", minioPassword)
//    .WaitFor(minio);

//var minioApiEndpoint = minio.GetEndpoint("minoioapi");


using var app = builder.Build();

await app.RunAsync();



////////////////////////////////////////////////////////////////////////////////////////////////////////////////
