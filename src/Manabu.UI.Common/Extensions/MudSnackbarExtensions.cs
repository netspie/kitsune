using MudBlazor;

namespace Manabu.UI.Common.Extensions;

public static class MudSnackbarExtensions
{
    public const int DefaultVisibleStateDuration = 2000;
    public static int VisibleStateDuration = DefaultVisibleStateDuration;

    public static void AddInfo(this ISnackbar snackbar, string message) =>
        snackbar.Add(message, Severity.Normal, opts =>
        {
            opts.VisibleStateDuration = VisibleStateDuration;
        });

    public static void AddError(this ISnackbar snackbar, string message) =>
        snackbar.Add(message, Severity.Error, opts =>
        {
            opts.VisibleStateDuration = VisibleStateDuration;
        });
}
