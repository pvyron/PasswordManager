using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using PasswordManager.Portal;
using PasswordManager.Portal.Services;
using System.Text.Json.Serialization;
using System.Text.Json;
using PasswordManager.Portal.Installers;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using MudBlazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 5000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Text;
});

builder.Services
    .AddHttpClient()
    .AddJsonOptions()
    .InstallServices()
    .AddBlazoredSessionStorage()
    .AddBlazoredLocalStorage();

await builder.Build().RunAsync();
