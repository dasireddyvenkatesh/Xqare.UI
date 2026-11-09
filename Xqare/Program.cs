using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Authorization;
using MudBlazor;
using MudBlazor.Services;
using Xqare;
using Xqare.BusinessLayer.Classes.Auth;
using Xqare.BusinessLayer.Classes.Common;
using Xqare.BusinessLayer.Classes.ContactUs;
using Xqare.BusinessLayer.Classes.PartnerShip;
using Xqare.BusinessLayer.Classes.Authorization;
using Xqare.BusinessLayer.Interfaces.Auth;
using Xqare.BusinessLayer.Interfaces.Admin;
using Xqare.BusinessLayer.Interfaces.Rbac;
using Xqare.BusinessLayer.Interfaces.Common;
using Xqare.BusinessLayer.Interfaces.ContactUs;
using Xqare.BusinessLayer.Interfaces.PartnerShip;
using Xqare.BusinessLayer.Interfaces.Profile;
using Xqare.BusinessLayer.Services.Common;
using Xqare.BusinessLayer.Services.Profile;
using Xqare.BusinessLayer.Services.Admin;
using Xqare.BusinessLayer.Services.Rbac;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"];

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl!)});

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IContactUsService, ContactUsService>();
builder.Services.AddScoped<IPartnershipService, PartnershipService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IRbacService, RbacService>();
builder.Services.AddScoped<RateLimitState>();
builder.Services.AddScoped<IApiClient, ApiClient>();
builder.Services.AddScoped<JwtAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(p =>
    p.GetRequiredService<JwtAuthStateProvider>());
builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy(PermissionPolicies.AdminOverviewView, policy =>
        policy.RequireAuthenticatedUser().AddRequirements(new PermissionRequirement("ADMIN_OVERVIEW_VIEW")));
    options.AddPolicy(PermissionPolicies.AdminUsersView, policy =>
        policy.RequireAuthenticatedUser().AddRequirements(new PermissionRequirement("ADMIN_USERS_VIEW")));
    options.AddPolicy(PermissionPolicies.AdminUsersManage, policy =>
        policy.RequireAuthenticatedUser().AddRequirements(new PermissionRequirement("ADMIN_USERS_MANAGE")));
    options.AddPolicy(PermissionPolicies.RbacRolesView, policy =>
        policy.RequireAuthenticatedUser().AddRequirements(new PermissionRequirement("RBAC_ROLES_VIEW")));
    options.AddPolicy(PermissionPolicies.RbacRolesManage, policy =>
        policy.RequireAuthenticatedUser().AddRequirements(new PermissionRequirement("RBAC_ROLES_MANAGE")));
    options.AddPolicy(PermissionPolicies.RbacPermissionsView, policy =>
        policy.RequireAuthenticatedUser().AddRequirements(new PermissionRequirement("RBAC_PERMISSIONS_VIEW")));
    options.AddPolicy(PermissionPolicies.RbacPermissionsManage, policy =>
        policy.RequireAuthenticatedUser().AddRequirements(new PermissionRequirement("RBAC_PERMISSIONS_MANAGE")));
    options.AddPolicy(PermissionPolicies.RbacAssignmentsManage, policy =>
        policy.RequireAuthenticatedUser().AddRequirements(new PermissionRequirement("RBAC_ASSIGNMENTS_MANAGE")));
});
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.MaxDisplayedSnackbars = 1;
    config.SnackbarConfiguration.PreventDuplicates = true;
    config.SnackbarConfiguration.ClearAfterNavigation = true;
    config.SnackbarConfiguration.VisibleStateDuration = 4000;
});
await builder.Build().RunAsync();
