using RogueLib.Dungeon;
using RogueLib.Utilities;

enum ArmourType
{
    LeatherArmour = 3,
    SteelArmour = 6,
    PlateArmour = 9,
    MithrilArmour = 12,
}

namespace RogueLib.Items
{
    
    public class Armour : Item
    {
        public int Defense { get; protected set; }

        public Armour(Vector2 pos, int defense) : base('X', pos, ConsoleColor.DarkGray)
        {

            Defense = defense;
        }

        public override void Draw(IRenderWindow disp)
        {
            disp.Draw(Glyph, Pos, Color);
        }

    }
}
