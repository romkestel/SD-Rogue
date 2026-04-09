using RogueLib.Utilities;

namespace RogueLib.Dungeon;

public abstract class Item : IDrawable
{
    public Item(char glyph, Vector2 pos)
    {
        Glyph = glyph;
        Pos = pos;
    }

   
    public char Glyph { get; init; }
    public Vector2 Pos { get; protected set; }
    public abstract void Draw(IRenderWindow disp);
}