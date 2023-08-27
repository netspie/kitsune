using Corelibs.Basic.UseCases;
using Manabu.UI.Common.Auth;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using static MudBlazor.CategoryTypes;

namespace Manabu.UI.Common.Components;

public abstract class CoreComponent : Microsoft.AspNetCore.Components.ComponentBase
{
    [Inject] public IQueryExecutor QueryExecutor { get; set; }
    [Inject] public ICommandExecutor CommandExecutor { get; set; }
    [Inject] public NavigationManager Navigation { get; set; }
    [Inject] public AuthenticationStateProvider Auth { get; set; }

    protected bool _isAdmin;
    protected bool _isEdit;

    protected override async Task OnInitializedAsync()
    {
        _isAdmin = await Auth.IsAdmin();
        await OnInitializedAsyncImpl();
    }

    protected virtual Task OnInitializedAsyncImpl() => Task.CompletedTask;
}
