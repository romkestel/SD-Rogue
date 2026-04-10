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
    public string Name;
    public Weapon(Vector2 pos, int damage) : base('V', pos)
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
      disp.Draw(Glyph, Pos, ConsoleColor.Blue);
   }
   
}