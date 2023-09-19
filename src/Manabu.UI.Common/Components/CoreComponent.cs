using Corelibs.Basic.Blocks;
using Corelibs.Basic.UseCases;
using Manabu.UI.Common.Auth;
using Manabu.UI.Common.Storage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Reflection;

namespace Manabu.UI.Common.Components;

public abstract class CoreComponent : ComponentBase
{
    [Inject] public IQueryExecutor QueryExecutor { get; set; }
    [Inject] public ICommandExecutor CommandExecutor { get; set; }
    [Inject] public NavigationManager Navigation { get; set; }
    [Inject] public AuthenticationStateProvider Auth { get; set; }
    [Inject] public IStorage Storage { get; set; }
    
    protected bool _isAdmin { get; private set; }

    private bool _isEditValue;
    protected event Func<bool, Task> OnEditModeChange;

    protected bool _isEdit { 
        get => _isEditValue; 
        set {
            _isEditValue = value;
            SetEditModeStored(value);
        }
    }

    protected bool IsEdit => _isAdmin && _isEdit;

    protected sealed override async Task OnInitializedAsync()
    {
        _isAdmin = await Auth.IsAdmin();

        await RefreshViewModel();
        await OnInitializedAsyncImpl();
    }

    protected virtual Task OnInitializedAsyncImpl() => Task.CompletedTask;
    protected virtual Task OnAfterRenderAsyncImpl(bool firstRender) => Task.CompletedTask;

    protected sealed override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _isEdit = await IsEditModeStored();

            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var stateItemProperties = properties.Where(p =>
                    p.PropertyType.BaseType != null &&
                    p.PropertyType.BaseType.IsGenericType &&
                    p.PropertyType.BaseType.GetGenericTypeDefinition() == typeof(StateItem<>))
                .ToArray();

            var stateItemTasks = stateItemProperties
                .Select(p => p.GetValue(this))
                .Where(p => p is not null)
                .Cast<IStateItem>()
                .Select(p => p.Init())
                .ToArray();

            await Task.WhenAll(stateItemTasks);
        }

        await OnAfterRenderAsyncImpl(firstRender);

        if (firstRender)
            await InvokeAsync(StateHasChanged);
    }

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

        if (OnEditModeChange is not null)
            await OnEditModeChange?.Invoke(value);
    }

    class EditModeEnabled
    {
        public bool Value { get; set; }
    }
}