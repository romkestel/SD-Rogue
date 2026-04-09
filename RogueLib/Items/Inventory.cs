using RogueLib.Dungeon;
using SandBox01.Levels.Items;

namespace RogueLib.Items;

public class Inventory
{
    private List<Item> _bag = new();
    public IReadOnlyList<Item> Bag => _bag.AsReadOnly();

    public int Count => _bag.Count;
    
    public bool AddItem(Item item)
    {
        if (item is Weapon && _bag.OfType<Weapon>().Count() >= 3)
        {
            return false;
        }

        if (item is Armour && _bag.OfType<Armour>().Count() >= 3)
        {
            return false;
        }

        if (item is Potion && _bag.OfType<Potion>().Count() >= 3)
        {
            return false;
        }
        _bag.Add(item);
        return true;
    }

    public void RemoveItem(Item item)
    {
        _bag.Remove(item);
    }
    
}