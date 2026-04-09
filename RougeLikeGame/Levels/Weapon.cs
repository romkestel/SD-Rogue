using RogueLib.Dungeon;
using RogueLib.Utilities;

namespace RlGameNS;

public class Weapon : Item
{
   public Weapon(Vector2 pos, int damage, char glyph) : base(glyph, pos)
   {
      Damage = damage;
   }
   
   public int Damage { get; set; }

   public override void Draw(IRenderWindow disp)
   {
      disp.Draw(Glyph, Pos, ConsoleColor.Gray);
   }
   
}