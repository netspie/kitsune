namespace Manabu.Entities.Shared;

public record class ItemMode(string Value)
{
    public static readonly ItemMode Original = new("original");
    public static readonly ItemMode Translation = new("translation");
}
