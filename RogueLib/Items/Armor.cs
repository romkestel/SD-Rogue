using RogueLib.Dungeon;
using RogueLib.Utilities;

public enum ArmourType
{
    LeatherArmour = 1,
    SteelArmour = 4,
    PlateArmour = 3,
    MithrilArmour = 7,
}

namespace RogueLib.Items
{
    public class Armour : Item, IEquatable<Armour>
    {
        public int Defense => (int)Type;
        public string Name => Type.ToString();
        
        public ArmourType Type { get; }
        
        // Auto rng / Auto generating constructor for easy Item generation
        public Armour(Vector2 pos, Random rng) : base('X', pos, ConsoleColor.DarkGray)
        {
            var values = Enum.GetValues<ArmourType>();
            Type = values[rng.Next(values.Length)];
        }
        
        public override void Draw(IRenderWindow disp)
        {
            disp.Draw(Glyph, Pos, Color);
        }
        
        // Implementing all of these to prevent reflection do to
        // all the LINQ beng applied to them in Inventory
        public bool Equals(Armour? other)
            => other is not null && Type == other.Type;
   
        public override bool Equals(object? obj)
            => Equals(obj as Armour);
   
        public override int GetHashCode()
            => Type.GetHashCode();
    }
}
