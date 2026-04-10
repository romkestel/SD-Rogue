using RogueLib.Utilities;

namespace RogueLib.Dungeon;

public abstract class Item : IDrawable
{
    public Item(char glyph, Vector2 pos, ConsoleColor color)
    {
        Glyph = glyph;
        Pos = pos;
        Color = color;
    }
    
    public ConsoleColor Color { get; init; }
    
    public char Glyph { get; init; }
    public Vector2 Pos { get; protected set; }
    public abstract void Draw(IRenderWindow disp);
}