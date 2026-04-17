using RogueLib.Dungeon;
using RogueLib.Utilities;

namespace RogueLib.Items;

public class HealthPotion : Potion
{
    public HealthPotion(Vector2 pos) : base(pos, ConsoleColor.Red) { }
    
    private static Random rng = new Random();
    
    public override void Drink(Player player)
    {
        int healAmount = rng.Next(1, (player.Hp / 6 + 1));
        player.Heal(healAmount);
    }

    public override void Draw(IRenderWindow disp)
    {
        disp.Draw(Glyph, Pos, Color);
    }
}