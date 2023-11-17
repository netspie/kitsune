using Microsoft.JSInterop;

namespace Manabu.UI.Common.Extensions;

public static class KeyHandlersExtensions
{
    public static ValueTask AddKeyDownEventHandler<T>(this IJSRuntime jsRuntime, DotNetObjectReference<T> @objectReference, string methodName)
        where T : class
    {
        return jsRuntime.InvokeVoidAsync("addDocumentKeyDownHandler", @objectReference, methodName);
    }
}
