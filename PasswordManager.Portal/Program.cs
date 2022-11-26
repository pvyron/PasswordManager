using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using PasswordManager.Portal;
using PasswordManager.Portal.Services;
using System.Text.Json.Serialization;
using System.Text.Json;
using PasswordManager.Portal.Installers;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

builder.Services
    .AddHttpClient()
    .AddJsonOptions()
    .InstallServices()
    .AddBlazoredLocalStorageAsSingleton();

await builder.Build().RunAsync();
