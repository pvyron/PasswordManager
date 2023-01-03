using PasswordManager.Infrastructure;
using System.Reflection;
using MediatR;
using PasswordManager.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddDataAccess(configuration)
        .InstallServices()
        .AddEndpointsApiExplorer()
        .AddSwaggerGen()
        .AddHttpContextAccessor();

services.AddMediatR(Assembly.GetAssembly(typeof(ApplicationAssembly))!);

//.AddMediator(o =>
//{
//    o.ServiceLifetime = ServiceLifetime.Transient;
//    o.Namespace = "PasswordManager.Application.MediatorCode";
//})

services.AddControllers();

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(
            authenticationScheme: JwtBearerDefaults.AuthenticationScheme,
            configureOptions: o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetValue<string>("AuthenticationServiceSettings:JwtKey")!)),
                    ValidIssuer = configuration.GetValue<string>("AuthenticationServiceSettings:JwtIssuer"),
                    ValidAudience= configuration.GetValue<string>("AuthenticationServiceSettings:JwtAudience"),
                    RequireExpirationTime = false,
                    ValidateIssuer = true,
                    ValidateAudience= true,
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
                          policy.WithOrigins("https://localhost:7210");
                          policy.AllowCredentials();
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
    policy.WithOrigins("https://localhost:7210");
    policy.AllowCredentials();
    policy.Build();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
