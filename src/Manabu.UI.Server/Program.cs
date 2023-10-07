using Corelibs.Basic.Corelibs.Basic.Net;
using Manabu.UI.Server;
using Manabu.UI.Server.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.Secure = CookieSecurePolicy.Always;
});

var useLocalIp = Environment.GetEnvironmentVariable("Kitsune_UseLocalIP") is not null ? true : false;
builder.Services.Configure<OpenIdConnectOptions>(
    OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.SaveTokens = true;
        options.ClientSecret = builder.Environment.IsProduction() ? Environment.GetEnvironmentVariable("KitsuneAuthSecret") : "";
        options.Scope.Add("offline_access");
        options.Scope.Add(options?.ClientId);
        options.Events.OnRedirectToIdentityProvider = async context =>
        {
            if (builder.Environment.IsProduction())
                context.ProtocolMessage.RedirectUri = "https://kitsunestudy.net/signin-oidc";
            else if (useLocalIp)
                context.ProtocolMessage.RedirectUri = $"{GetLocalAddress()}/signin-oidc";
        };
        options.Events.OnRedirectToIdentityProviderForSignOut = async context =>
        {
            if (builder.Environment.IsProduction())
                context.ProtocolMessage.PostLogoutRedirectUri = "https://kitsunestudy.net/";
            else if(useLocalIp)
                context.ProtocolMessage.PostLogoutRedirectUri = GetLocalAddress();
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddMicrosoftIdentityConsentHandler();
builder.Services.AddMudServices();
builder.Services.AddHttpContextAccessor();

builder.Services.InitializeApp(builder.Environment);

var app = builder.Build();

app.UseHttpsRedirection();
if (!app.Environment.IsDevelopment())
    app.UseHsts();

app.UseCookiePolicy();
app.UseStaticFiles();
app.UseStaticFilesEx("media");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

string GetLocalAddress() =>
    $"https://{NetSocketExtensions.GetLocalIPAddress()}:7073";
