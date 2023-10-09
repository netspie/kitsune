using Corelibs.Basic.Repository;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Manabu.UI.Common.Auth;

public class MockPrincipalAccessor : IAccessorAsync<ClaimsPrincipal>
{
    public static readonly Claim DefaultClaim = new(ClaimTypes.NameIdentifier, "user-default");

    private readonly AuthenticationStateProvider _auth;

    public MockPrincipalAccessor(AuthenticationStateProvider auth)
    {
        _auth = auth;
    }

    public async Task<ClaimsPrincipal> Get() =>
        new ClaimsPrincipal(new ClaimsIdentity[]
        {
            new ClaimsIdentity(new Claim[] { DefaultClaim })
        });
}
