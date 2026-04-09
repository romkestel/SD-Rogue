using RogueLib.Dungeon;
using RogueLib.Utilities;

namespace SandBox01.Levels.Items;

public abstract class Potion : Item
{
    protected Potion(Vector2 pos) : base('i', pos) { }

    public abstract void Drink(Player player);
}