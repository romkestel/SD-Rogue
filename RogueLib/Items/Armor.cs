using RogueLib.Dungeon;
using RogueLib.Utilities;

namespace RogueLib.Items
{
    public class Armour : Item
    {
        public int Defense { get; protected set; }

        public Armour(Vector2 pos, int defense, char glyph) : base('X', pos, ConsoleColor.DarkGray)
        {
            Defense = defense;
        }

        public override void Draw(IRenderWindow disp)
        {
            disp.Draw(Glyph, Pos, Color);
        }

    }
}
