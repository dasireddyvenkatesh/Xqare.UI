using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Xqare;
using Xqare.BusinessLayer.Classes;
using Xqare.BusinessLayer.Classes.Auth;
using Xqare.BusinessLayer.Classes.Common;
using Xqare.BusinessLayer.Classes.ContactUs;
using Xqare.BusinessLayer.Classes.PartnerShip;
using Xqare.BusinessLayer.Interfaces.Auth;
using Xqare.BusinessLayer.Interfaces.Common;
using Xqare.BusinessLayer.Interfaces.ContactUs;
using Xqare.BusinessLayer.Interfaces.PartnerShip;
using Xqare.BusinessLayer.Services.Common;
using Xqare.BusinessLayer.Validators;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
builder.Services.AddSingleton<VisitCounterService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IContactUsService, ContactUsService>();
builder.Services.AddScoped<IPartnershipService, PartnershipService>();
builder.Services.AddScoped<IApiClient, ApiClient>();
builder.Services.AddScoped<JwtAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(p =>
    p.GetRequiredService<JwtAuthStateProvider>());
builder.Services.AddAuthorizationCore();
builder.Services.AddMudServices();
await builder.Build().RunAsync();
