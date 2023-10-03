namespace Manabu.Entities.Rehearse.RehearseItems;

public record Difficulty(int Value)
{
    public static readonly Difficulty Impossible = new(0);
    public static readonly Difficulty Hard = new(1);
    public static readonly Difficulty Advanced = new(2);
    public static readonly Difficulty Challenging = new(3);
    public static readonly Difficulty Easy = new(4);
    public static readonly Difficulty Obvious = new(5);

    public bool IsValid() => Value >= 0 && Value <= 5;

    public static bool operator >(Difficulty left, Difficulty right) =>
        left.Value > right.Value;

    public static bool operator <(Difficulty left, Difficulty right) =>
        left.Value < right.Value;

    public static bool operator >=(Difficulty left, Difficulty right) =>
        left.Value >= right.Value;

    public static bool operator <=(Difficulty left, Difficulty right) =>
        left.Value <= right.Value;

    public static Difficulty operator +(Difficulty left, Difficulty right) =>
        new Difficulty(left.Value + right.Value);

    public static Difficulty operator -(Difficulty left, Difficulty right) =>
        new Difficulty(left.Value - right.Value);

    public static Difficulty operator +(Difficulty left, int right) =>
        new Difficulty(left.Value + right);

    public static Difficulty operator -(Difficulty left, int right) =>
        new Difficulty(left.Value - right);

    public static bool operator >(Difficulty left, int right) =>
        left.Value > right;

    public static bool operator <(Difficulty left, int right) =>
        left.Value < right;

    public static bool operator >=(Difficulty left, int right) =>
        left.Value >= right;

    public static bool operator <=(Difficulty left, int right) =>
        left.Value <= right;

    public static int operator -(int left, Difficulty right) =>
        left - right.Value;

    public static int operator +(int left, Difficulty right) =>
        left + right.Value;

}