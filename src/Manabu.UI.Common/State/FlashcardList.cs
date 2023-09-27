namespace Manabu.UI.Common.State;

public class FlashcardList
{
    public Flashcard[] Items { get; set; }

    public record Flashcard(string ItemId, string? RehearseItemId = null, string? ItemType = null, string? Mode = null);
}
