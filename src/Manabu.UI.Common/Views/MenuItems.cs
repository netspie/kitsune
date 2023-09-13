namespace Manabu.UI.Common.Views;

public static class MenuItems
{
    public static readonly MenuItem Add = new MenuItem("add", "Add", OnlyAdmin: true);
    public static readonly MenuItem AddChildAsFirst = new MenuItem("addChildAsFirst", "Add Child as First", OnlyAdmin: true);
    public static readonly MenuItem AddChildAsLast = new MenuItem("addChildAsLast", "Add Child as Last", OnlyAdmin: true);
    public static readonly MenuItem AddSibling = new MenuItem("addSibling", "Add Sibling", OnlyAdmin: true);

    public static readonly MenuItem Remove = new MenuItem("remove", "Remove", OnlyAdmin: true);

    public static readonly MenuItem Move = new MenuItem("move", "Move", OnlyAdmin: true);

    public static readonly MenuItem MoveUp = new MenuItem("moveUp", "Move Up", OnlyAdmin: true);
    public static readonly MenuItem MoveDown = new MenuItem("moveDown", "Move Down", OnlyAdmin: true);

    public static readonly MenuItem Cut = new MenuItem("cut", "Cut", OnlyAdmin: true);
    public static readonly MenuItem Paste = new MenuItem("paste", "Paste", OnlyAdmin: true);

    public static readonly MenuItem Select = new MenuItem("select", "Select", OnlyAdmin: true);
}
