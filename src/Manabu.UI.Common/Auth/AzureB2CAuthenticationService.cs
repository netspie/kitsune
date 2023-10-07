using Corelibs.Basic.Auth;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Manabu.UI.Common.Auth;

public sealed class AzureB2CAuthenticationService : IAuthenticationService
{
    private readonly AuthenticationStateProvider _auth;

    public AzureB2CAuthenticationService(AuthenticationStateProvider auth)
    {
        _auth = auth;
    }

    public async Task<string> GetLabel()
    {
        var state = await _auth.GetAuthenticationStateAsync();
        if (!state.User.Identity.IsAuthenticated)
            return "Sign In";

        return "Sign Out";
    }

    public async Task<bool> IsAdmin()
    {
        var state = await _auth.GetAuthenticationStateAsync();
        return state.User.IsAdmin();
    }

    public async Task<bool> IsSignedIn()
    {
        var state = await _auth.GetAuthenticationStateAsync();
        return state.User.Identity.IsAuthenticated;
    }

    public async Task SignAction(NavigationManager navigationManager)
    {
        var state = await _auth.GetAuthenticationStateAsync();
        if (!state.User.Identity.IsAuthenticated)
            navigationManager.NavigateTo($"MicrosoftIdentity/Account/SignIn", forceLoad: true);
        else
            navigationManager.NavigateTo($"MicrosoftIdentity/Account/SignOut", forceLoad: true);
    }
}
