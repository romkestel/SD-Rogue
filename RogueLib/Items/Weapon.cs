using RogueLib.Dungeon;
using RogueLib.Utilities;

namespace RogueLib.Items;

public class Weapon : Item
{
   public Weapon(Vector2 pos, int damage, char glyph) : base('T', pos, ConsoleColor.Gray)
   {       
      Damage = damage;
   }
   
   public int Damage { get; set; }

   public override void Draw(IRenderWindow disp)
   {
      disp.Draw(Glyph, Pos, Color);
   }
   
}