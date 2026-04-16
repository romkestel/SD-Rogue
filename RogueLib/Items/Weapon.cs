using RogueLib.Dungeon;
using RogueLib.Utilities;

namespace RogueLib.Items;

public enum WeaponType
{
    Spear = 3,
    Sword = 4,
    Axe = 5,
    GreatSword = 6,
    Mace = 8,
}

public class Weapon : Item, IEquatable<Weapon>
{ 
   public string Name => Type.ToString();
   public WeaponType Type { get; }
   public int Damage => (int)Type;
   
   // Auto rng / Auto generating constructor for easy Item generation
   public Weapon(Vector2 pos, Random rng) : base('T', pos, ConsoleColor.Gray)
   {
      var values = Enum.GetValues<WeaponType>();
      Type = values[rng.Next(values.Length)];
   }
   
   // Full constructor just in case
   public Weapon(Vector2 pos, WeaponType type) : base('T', pos, ConsoleColor.Gray)
   {
      Type = type;
   }
   
   public override void Draw(IRenderWindow disp)
   {
      disp.Draw(Glyph, Pos, Color);
   }
   
   // Implementing all of these to prevent reflection do to
   // all the LINQ beng applied to them in Inventory
   public bool Equals(Weapon? other)
      => other is not null && Type == other.Type;
   
   public override bool Equals(object? obj)
      => Equals(obj as Weapon);
   
   public override int GetHashCode()
      => Type.GetHashCode();
}