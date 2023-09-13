using Corelibs.Basic.Collections;

namespace Manabu.UI.Common.Components;

internal static class EditableListExtensions
{
    public static string GetIndexStr(this EditableList.TreeItemData item, List<EditableList.TreeItemData> rootItems)
    {
        return item.GetIndexes(rootItems)
            .Select(i => (i + 1).ToString())
            .AggregateOrDefault((x, y) => $"{x}.{y}");
    }

    public static int[] GetIndexes(this EditableList.TreeItemData item, List<EditableList.TreeItemData> rootItems) =>
        item.Flatten(i =>
        {
            if (i?.Parent is null)
                return Array.Empty<EditableList.TreeItemData>();

            return i.Parent.SingleToArray();
        })
        .Append(item)
        .Select(i =>
        {
            if (i.Parent is null)
                return rootItems.IndexOf(i);

            return i.Index;
        })
        .ToArray();
}
