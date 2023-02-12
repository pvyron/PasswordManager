using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using LanguageExt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.Infrastructure;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

if (!builder.Environment.IsDevelopment())
{
    var keyVaultEndPoint = Environment.GetEnvironmentVariable("KEYVAULT_ENDPOINT");

    if (keyVaultEndPoint is null)
        throw new ValueIsNullException($"Invalid configuration, missing value for KEYVAULT_ENDPOINT");

    var secretClient = new SecretClient(new(keyVaultEndPoint), new DefaultAzureCredential());

    configuration.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
}

services.AddDataAccess(configuration)
        .InstallServices(configuration)
        .AddEndpointsApiExplorer()
        .AddSwaggerGen()
        .AddHttpContextAccessor();

services.AddMediator(o =>
{
    o.Namespace = "PasswordManager.Application.MediatorCode";
    o.ServiceLifetime = ServiceLifetime.Transient;
});

services.AddControllers();

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(
            authenticationScheme: JwtBearerDefaults.AuthenticationScheme,
            configureOptions: o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetValue<string>("AuthorizationService:JwtKey")!)),
                    ValidIssuer = configuration.GetValue<string>("AuthorizationService:JwtIssuer"),
                    ValidAudience = configuration.GetValue<string>("AuthorizationService:JwtAudience"),
                    RequireExpirationTime = false,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true
                };
                o.IncludeErrorDetails = true;
            });

services.AddCors(o =>
{
    o.AddDefaultPolicy(policy =>
                      {
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                          policy.AllowAnyOrigin();
                          policy.Build();
                      });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy =>
{
    policy.AllowAnyHeader();
    policy.AllowAnyMethod();
    policy.AllowAnyOrigin();
    policy.Build();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/heartbeat", () =>
{
    return "Awake";
});

app.MapControllers();

app.Run();
