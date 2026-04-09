using RogueLib.Dungeon;
using RogueLib.Utilities;

namespace RlGameNS;

public class Gold : Item
{
    public Gold(Vector2 pos, int amount) : base('*', pos)
    {
        Amount = amount;
    }
    public int Amount { get; init; }
    
    public override void Draw(IRenderWindow disp)
    {
        disp.Draw(Glyph, Pos, ConsoleColor.Yellow);
    }
}