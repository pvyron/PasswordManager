using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using PasswordManager.Portal;
using PasswordManager.Portal.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

builder.Services.AddHttpClient();
builder.Services.AddSingleton<ClientStateData>();
builder.Services.AddSingleton<ApiClient>();
builder.Services.AddSingleton<AuthenticationService>();


await builder.Build().RunAsync();
