namespace Manabu.UI.Common.Views;

public record MenuItem(
    string Id,
    string Name, 
    string IdSecondary = "",
    bool OnlyAdmin = false);
