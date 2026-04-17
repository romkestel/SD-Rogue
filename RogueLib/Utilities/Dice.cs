namespace RogueLib.Utilities;

public static class Dice
{
    public static int Roll(int sides)
    {
        if (sides < 1) return 0;
        return Random.Shared.Next(1, sides + 1);
    }
    // Makes it possible to have 2d6 or 3d8 or other dice patterns
    public static int Roll(int count, int sides)
    {
        if (count < 1 || sides < 1) return 0;

        int total = 0;
        for (int i = 0; i < count; i++)
        {
            total += Roll(sides);
        }
        return total;
    }
}