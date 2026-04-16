using RogueLib.Dungeon;
using RogueLib.Items;
using RogueLib.Utilities;
using RogueLib.Utilities.Enemies;


public abstract class Player : IActor, IDrawable {
   public string       Name { get; set; }
   public Vector2      Pos;
   public char         Glyph => '@';
   public ConsoleColor _color = ConsoleColor.White;

   protected int _level  = 0;
   public int _hp     = 12;
   protected int _str    = 16;
   protected int _arm    = 4;
   protected int _exp    = 0;
   protected int _gold   = 0;
   protected int _maxHp  = 12;
   protected int _maxStr = 16;
   protected int _turn   = 0;
   
   public int Str => _str + (_equippedWeapon?.Damage ?? 0);
   public int Arm => _arm + (_equippedArmour?.Defense ?? 0);

   public Inventory Inventory { get; } = new();

   public int Hp { get => _hp; }
   public int MaxHp { get => _maxHp; }
   private Weapon? _equippedWeapon;
   private Armour? _equippedArmour;
   
   // Properties for updating the HUD
   public string EquippedWeaponName => _equippedWeapon?.Name ?? "None";
   public string EquippedArmourName => _equippedArmour?.Name ?? "None";
   public int EquippedWeaponDamage => _equippedWeapon?.Damage ?? 0;
   public int EquippedArmourDefense => _equippedArmour?.Defense ?? 0;
   
   // IF the player has NO equipped items, it will automatically place it in
   // the _equippedWeapon or _equippedArmour slots
   public bool TryAutoPickup(Item item)
   {
      if (item is Weapon w && _equippedWeapon == null)
      {
         _equippedWeapon = w;
         return true;
      }

      if (item is Armour a && _equippedArmour == null)
      {
         _equippedArmour = a;
         return true;
      }
      return false;
   }

   // Will be used in Level so the player can cycle through their
   // looted weapons to change their _currentWeapon using a Keybind
   public bool CycleWeapon()
   {
      var weapons = Inventory.Weapons.ToList();
      if(!weapons.Any()) return false;

      if (_equippedWeapon is null || !weapons.Contains(_equippedWeapon))
      {
         _equippedWeapon = weapons.FirstOrDefault();
         return _equippedWeapon is not null;
      }

      var idx = weapons.IndexOf(_equippedWeapon);
      _equippedWeapon = weapons[(idx + 1) % weapons.Count];
      return true;
   }

   // Will be used in Level so the player can cycle through their
   // looted armour to change their _currentArmour using a Keybind
   public bool CycleArmour()
   {
      var armours = Inventory.Armours.ToList();
      if (!armours.Any()) return false;

      if (_equippedArmour is null || !armours.Contains(_equippedArmour))
      {
         _equippedArmour = armours.FirstOrDefault();
         return _equippedArmour is not null;
      }
      
      var idx = armours.IndexOf(_equippedArmour);
      _equippedArmour = armours[(idx + 1) % armours.Count];
      return true;
   }

   // Drinks the first potion in the players inventory then disposes of it
   public bool DrinkFirstPotion()
   {
      var potion = Inventory.Potions.FirstOrDefault();
      if (potion is null) return false;
      
      potion.Drink(this);
      Inventory.RemoveItem(potion);
      return true;
   }
   
   
   public void AddGold(int amount)
   {
      _gold += amount;
   }    
   
   public void Heal(int amount)
   {
      _hp += amount;
      if (_hp > _maxHp)
      {
         _hp = _maxHp;
      }
   }

   public void BuffDamage(int amount)
   {
      _str += amount;
      if (_str > _maxStr)
      {
         _str = _maxStr;
      }
   }
   
   public int Turn => _turn;

   public Player() {
      Name = "Rogue";
      Pos  = Vector2.Zero;
   }

   public string HUD 
   {
      get
      {
         var line1 = $"Level:{_level}  Gold:{_gold}  Hp:{_hp}({_maxHp})  Str:{Str}  Arm:{Arm}  Exp:{_exp}/10  Turn:{_turn}".PadRight(78);
         var line2 = $"Wpn: {EquippedWeaponName} ({EquippedWeaponDamage})   Arm: {EquippedArmourName} ({EquippedArmourDefense})".PadRight(78); 
         return line1 + "\r\n" + line2;
      }
   }
      
   
    public void Attack(Enemy enemy)
    {
    }
    // Increase defense depending on armour type
    public void ItemEquipped(Weapon weaponType, Armour armourType)
    {
        _arm += armourType.Defense;
    }

    public void TakeDamage(int damage)
    {
        int damageTaken = Math.Max(0, damage - _arm);
        _hp -= damageTaken;
        
    }

    public virtual void Update() {
       
      _turn++;
   }

   public virtual void Draw(IRenderWindow disp) {
      disp.Draw(Glyph, Pos, _color);
   }
}