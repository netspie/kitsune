using Corelibs.Basic.Blocks;
using Corelibs.Basic.UseCases;
using Manabu.UI.Common.Auth;
using Manabu.UI.Common.Storage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Manabu.UI.Common.Components;

public abstract class CoreComponent : Microsoft.AspNetCore.Components.ComponentBase
{
    [Inject] public IQueryExecutor QueryExecutor { get; set; }
    [Inject] public ICommandExecutor CommandExecutor { get; set; }
    [Inject] public NavigationManager Navigation { get; set; }
    [Inject] public AuthenticationStateProvider Auth { get; set; }
    [Inject] public IStorage Storage { get; set; }
    
    protected bool _isAdmin { get; private set; }

    private bool _isEditValue;

    protected bool _isEdit { 
        get => _isEditValue; 
        set {
            _isEditValue = value;
            SetEditModeStored(value);
        }
    }

    protected bool IsEdit => _isAdmin && _isEdit;

    protected override async Task OnInitializedAsync()
    {
        _isAdmin = await Auth.IsAdmin();
        _isEdit = await IsEditModeStored();

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

    private async Task<bool> IsEditModeStored()
    {
        var stored = await Storage.Get<EditModeEnabled>();
        return stored is not null ? stored.Value : false;
    }
    protected async Task SetEditModeStored(bool value)
    {
        await Storage.Save(new EditModeEnabled
        {
            Value = value
        });
    }

    class EditModeEnabled
    {
        public bool Value { get; set; }
    }
}