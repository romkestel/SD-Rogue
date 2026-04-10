using RogueLib.Dungeon;
using RogueLib.Utilities;

namespace RogueLib.Items;

enum WeaponType
{
    Sword = 5,
    Axe = 7,
    GreatSword = 10,
    TwoHandedAxe = 13
}
public class Weapon : Item
{
   public Weapon(Vector2 pos, int damage, char glyph) : base('T', pos, ConsoleColor.Gray)
   {       
      Damage = damage;
   }

    //public void WeaponType()
    //{
    //    Name = type.ToString();
    //    Damage = (int)type;
    //}

    public int Damage { get; set; }

   public override void Draw(IRenderWindow disp)
   {
      disp.Draw(Glyph, Pos, Color);
   }
   
}