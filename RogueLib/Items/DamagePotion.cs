using RogueLib.Dungeon;
using RogueLib.Utilities;

namespace RogueLib.Items;

public class DamagePotion : Potion
{
    public DamagePotion(Vector2 pos) : base(pos, ConsoleColor.Blue) { }
    

    public override void Drink(Player player)
    {
        player.BuffDamage(1);
    }

    public override void Draw(IRenderWindow disp)
    {
        disp.Draw(Glyph, Pos, Color);
    }
}