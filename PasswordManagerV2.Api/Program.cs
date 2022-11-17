using MediatR;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string shopDbConnectionString = builder.Configuration.GetConnectionString("ShopDbConnectionString");
builder.Services.RegisterApplication(shopDbConnectionString);

builder.Services.AddMediatR(typeof(Program));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
