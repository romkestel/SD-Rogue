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
    protected int dmge;

   public Inventory Bag { get; } = new();

   public int Hp { get => _hp; }
   public int MaxHp { get => _maxHp; }

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

   public string HUD =>
      $"Level:{_level}  Gold: {_gold}    Hp: {_hp}({_maxHp})" +
      $"  Str: {_str}({_maxStr})" +
      $"  Arm: {_arm}   Exp: {_exp}/{10} Turn: {_turn}";

    public void Attack(Enemy enemy)
    {
        Console.WriteLine($"Player attacks with Big Sword");
      
    }
    // Increase defense depending on armour type
    public void ItemEquipped(Weapon weaponType, Armour armourType)
    {
        dmge += weaponType.Damage;
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