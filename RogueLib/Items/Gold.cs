using RogueLib.Dungeon;
using RogueLib.Utilities;

namespace RogueLib.Items;

public class Gold : Item
{
    public Gold(Vector2 pos, int amount) : base('*', pos, ConsoleColor.Yellow)
    {
        Amount = amount;
    }
    public int Amount { get; init; }
    
    public override void Draw(IRenderWindow disp)
    {
        disp.Draw(Glyph, Pos, Color);
    }
}