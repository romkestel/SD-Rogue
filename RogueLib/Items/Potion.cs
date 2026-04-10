using RogueLib.Dungeon;
using RogueLib.Utilities;
namespace RogueLib.Items;

public abstract class Potion : Item
{
    protected Potion(Vector2 pos, ConsoleColor color) : base('i', pos, color) { }
    
    public abstract void Drink(Player player);
}