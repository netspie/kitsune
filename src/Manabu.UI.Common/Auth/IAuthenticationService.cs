using Microsoft.AspNetCore.Components;

namespace Manabu.UI.Common.Auth;

public interface IAuthenticationService
{
    Task<bool> IsSignedIn();
    Task<string> GetLabel();
    Task SignAction(NavigationManager navigationManager);
    Task<bool> IsAdmin();
}
