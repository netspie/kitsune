namespace Manabu.UI.Common.Views;

public static class MenuItems
{
    public static readonly MenuItem Add = new MenuItem("add", "Add");
    public static readonly MenuItem AddChildAsFirst = new MenuItem("addChildAsFirst", "Add Child as First");
    public static readonly MenuItem AddChildAsLast = new MenuItem("addChildAsLast", "Add Child as Last");
    public static readonly MenuItem AddSibling = new MenuItem("addSibling", "Add Sibling");

    public static readonly MenuItem Remove = new MenuItem("remove", "Remove");

    public static readonly MenuItem MoveUp = new MenuItem("moveUp", "Move Up");
    public static readonly MenuItem MoveDown = new MenuItem("moveDown", "Move Down");
}
