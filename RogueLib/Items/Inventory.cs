using RogueLib.Dungeon;

namespace RogueLib.Items;

public class Inventory
{
    // Encapsulated private readonly List exposed through an IReadOnlyList
    // To prevent outside classes from wiping the List of elements
    private readonly List<Item> _bag = new();
    public IReadOnlyList<Item> Bag => _bag.AsReadOnly();

    public int Count => _bag.Count;
    
    // "helper" collection for easier access to specific sets of Items 
    // in the inventory
    public IEnumerable<Weapon> Weapons => _bag.OfType<Weapon>();
    public IEnumerable<Armour> Armours => _bag.OfType<Armour>();
    public IEnumerable<Potion> Potions => _bag.OfType<Potion>();
    
    // Checks the Item type and the Count of said type through the IEnumerable
    // properties
    public bool AddItem(Item item)
    {
        if (item is Weapon && Weapons.Count() >= 3) return false;
        if (item is Armour && Armours.Count() >= 3) return false;
        if (item is Potion && Potions.Count() >= 3) return false;
        _bag.Add(item);
        return true;
    }
    
    // For discarding unwanted loot
    public void RemoveItem(Item item)
    {
        _bag.Remove(item);
    }
    
}