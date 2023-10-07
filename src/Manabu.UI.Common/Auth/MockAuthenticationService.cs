using Microsoft.AspNetCore.Components;

namespace Manabu.UI.Common.Auth;

public sealed class MockAuthenticationService : IAuthenticationService
{
    private static bool _isSignedIn;

    public async Task<string> GetLabel() => await IsSignedIn() ? "Sign In" : "Sign Out";

    public Task<bool> IsAdmin() => IsSignedIn();

    public async Task<bool> IsSignedIn() => _isSignedIn;

    public async Task SignAction(NavigationManager navigationManager)
    {
        var isSignedIn = await IsSignedIn();
        _isSignedIn = !isSignedIn;
        navigationManager.NavigateTo(navigationManager.Uri, forceLoad: true);
    }
}
