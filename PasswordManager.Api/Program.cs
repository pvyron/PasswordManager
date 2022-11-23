using PasswordManager.Infrastructure;
using System.Reflection;
using MediatR;
using PasswordManager.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;



services.InstallServices()
        .AddEndpointsApiExplorer()
        .AddSwaggerGen();

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
