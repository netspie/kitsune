using Corelibs.Basic.Blocks;
using Corelibs.Basic.UseCases;
using Manabu.Entities.Phrases;
using Manabu.UI.Common.Auth;
using Manabu.UseCases.Phrases;
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

        await RefreshViewModel();
        await OnInitializedAsyncImpl();
    }

    protected virtual Task OnInitializedAsyncImpl() => Task.CompletedTask;

    protected async Task<bool> ExecuteAdminViewAction(Func<Task<Result>> action)
    {
        if (!_isAdmin)
            return false;

        var result = await action();

        await RefreshView();

        return result.IsSuccess;
    }

    protected async Task<bool> RefreshView()
    {
        await RefreshViewModel();
        await InvokeAsync(StateHasChanged);

        return true;
    }

    protected virtual Task RefreshViewModel() { return Task.CompletedTask; }
}
