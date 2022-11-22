using PasswordManager.Infrastructure;
using System.Reflection;
using MediatR;
using PasswordManager.Application;

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
