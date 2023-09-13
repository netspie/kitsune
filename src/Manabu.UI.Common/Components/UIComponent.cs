using Microsoft.AspNetCore.Components;

namespace Manabu.UI.Common.Components;

public abstract class UIComponent : ComponentBase
{
    public static Func<Task> InvokeVoid(Delegate del, params object[] args)
    {
        return () =>
        {
            if (del is null)
                return Task.CompletedTask;

            var task = del.DynamicInvoke(args) as Task;
            if (task is null)
                return Task.CompletedTask;

            return task;
        };
    }

    public static Func<Task<bool>> InvokeBool(Delegate del, object arg1, object arg2, object arg3) =>
        InvokeBool(del, new object[] { arg1, arg2, arg3 });

    public static Func<Task<bool>> InvokeBool(Delegate del, object arg1, object arg2) =>
        InvokeBool(del, new object[] { arg1, arg2 });

    public static Func<Task<bool>> InvokeBool(Delegate del, object arg1) =>
        InvokeBool(del, new object[] { arg1 });

    private static Func<Task<bool>> InvokeBool(Delegate del, object[] args)
    {
        return () =>
        {
            if (del is null)
                return Task.FromResult(true);

            var task = del.DynamicInvoke(args) as Task<bool>;
            if (task is null)
                return Task.FromResult(true);

            return task;
        };
    }

    public static Func<Task<T>> Invoke<T>(Delegate del, params object[] args)
    {
        return () =>
        {
            if (del is null)
                return Task.FromResult<T>(default);

            var task = del.DynamicInvoke(args) as Task<T>;
            if (task is null)
                return Task.FromResult<T>(default);

            return task;
        };
    }
}
