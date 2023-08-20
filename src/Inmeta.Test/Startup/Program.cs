using Azure.Identity;
using Mapster;
using Inmeta.Test.Startup.Extensions;
using Inmeta.Test.Startup.Factories;
using Inmeta.Test.Services;

var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsProduction())
{
    builder.Configuration.AddAzureKeyVault(
        new Uri(builder.Configuration["AppSettings:AzureKeyVault:VaultName"]!),
        new DefaultAzureCredential()
    );
}

var hashidsService = new HashidsService(builder.Configuration);
var jsonOptions = JsonSerializerOptionsFactory.Create();

builder.Services.ConfigureData(builder.Configuration);
builder.Services.ConfigureServices(builder.Configuration, hashidsService, jsonOptions);
builder.Services.ConfigureApi(builder.Configuration, jsonOptions);
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureAuthorization();

TypeAdapterConfig.GlobalSettings.ConfigureMapping(hashidsService);

var app = builder.Build();
app.Configure();
app.Run();
