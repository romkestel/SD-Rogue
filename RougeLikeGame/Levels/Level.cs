using RogueLib.Dungeon;
using RogueLib.Engine;
using RogueLib.Items;
using RogueLib.Utilities;
using RogueLib.Utilities.Enemies;
using TileSet = System.Collections.Generic.HashSet<RogueLib.Utilities.Vector2>;

namespace SandBox01.Levels;

// -----------------------------------------------------------------------
// The Level is the model, all the game world objects live in the model. 
// player input updates the model, the model updates the view, and the 
// controller runs the whole thing. 
//
// Scene is the base class for all game scenes (levels). Scene is an 
// abstract class that implements IDrawable and ICommandable. 
// 
// A dungeon level is a collection or rooms and tunnels in a 78x25 grid. 
// each tile is at a point, or grid location, represented by a Vector2. 
// 
// *TileSets* are HashSets of grid points, TileSets can be used to tell 
// GameScreen what tiles to draw. TileSets can be combined with Union and 
// Intersect to create complex tile sets.
// -----------------------------------------------------------------------
public class Level : Scene {
   // ---- level config ---- 
   protected string? _map;
   protected int     _senseRadius = 8;
   
   // --- Tile Sets -----
   // used to keep track of state of tiles on the map
   protected TileSet _walkables; // walkable tiles 
   protected TileSet _floor;
   protected TileSet _tunnel;
   protected TileSet _door;
   protected TileSet _decor; // walls and other decorations, always visible once discovered

   protected TileSet _discovered; // tiles the player has seen
   protected TileSet _inFov;      // current fov of player

   protected List<Item> _items;
   protected List<Enemy> _enemies;
   
   
   public Level(Player p, string map, Game game) {
      if (game == null || p == null || map == null)
         throw new ArgumentNullException("game, player, or map cannot be null");

      _player     = p;
      _player.Pos = new Vector2(4, 12); // random, or at stairs
      _map        = map;
      _game       = game; // _game
      _items      = new List<Item>(); // type of item
      _enemies   = new List<Enemy>();
      
      

        initMapTileSets(map);
        updateDiscovered();
        registerCommandsWithScene();
        SpawnEnemy();
        SpreadItem();

        // Spawns Items
        void SpreadItem()
      {
         var rng = new Random();
         var am = rng.Next(10, 20);
         
         var wep = rng.Next(1, 3);
         var armour = rng.Next(1, 3);
         var hPotion = rng.Next(1, 3);
         var strPotion = rng.Next(0, 1);
         

            for (int i = 0; i < am; i++)
            {
                var tile = _floor.ElementAt(rng.Next(_floor.Count));
                _items.Add(new Gold(tile, rng.Next(2, 61)));
            }

            for (int i = 0; i < wep; i++)
            {
                var tile = _floor.ElementAt(rng.Next(_floor.Count));
                _items.Add(new Weapon(tile, rng));
            }

            for (int i = 0; i < armour; i++)
            {
                var tile = _floor.ElementAt(rng.Next(_floor.Count));
                _items.Add(new Armour(tile, rng));
            }

            for (int i = 0; i < hPotion; i++)
            {
               var tile = _floor.ElementAt(rng.Next(_floor.Count));
               _items.Add(new HealthPotion(tile));
            }

            for (int i = 0; i < strPotion; i++)
            {
               var tile = _floor.ElementAt(rng.Next(_floor.Count));
               _items.Add(new DamagePotion(tile));
            }
      }

        // Spawns Enemies
        void SpawnEnemy()
        {
            var rng = new Random();
            var goblins = rng.Next(3, 7);
            var orcs = rng.Next(2, 5);
            var trolls = rng.Next(1, 4);

            for (int i = 0; i < goblins; i++)
            {
                var tile = _floor.ElementAt(rng.Next(_floor.Count));
                _enemies.Add(new Goblin (tile));
            }
            for (int i = 0; i < orcs; i++)
            {
                var tile = _floor.ElementAt(rng.Next(_floor.Count));
                _enemies.Add(new Orc(tile));
            }
            for (int i = 0; i < trolls; i++)
            {
                var tile = _floor.ElementAt(rng.Next(_floor.Count));
                _enemies.Add(new Troll(tile));
            }
        }
   }

   protected void updateDiscovered() {
      _inFov = fovCalc(_player!.Pos, _senseRadius);

      if (_discovered is null)
         _discovered = new TileSet();

      _discovered.UnionWith(_inFov);
   }

   protected TileSet fovCalc(Vector2 pos, int sens)
      => Vector2.getAllTiles().Where(t => (pos - t).RookLength < sens).ToHashSet();

    // -----------------------------------------------------------------------
    
    public void EnemyPov() // -- TEAM
    {
        int hitCount = 0;
        int missCount = 0;
        int totalDamage = 0;
        foreach (var e in _enemies.ToList())
        {
            // I need a method for the enemy to chase the player when
            // within enemy radius BUT it needs to only chase the
            // player WITHIN walkable tiles
            
            var dist = (e.Pos - _player.Pos).KingLength;
            
            //If adjacent, attack instead of moving.
            if (dist == 1)
            {

                int enemyDmg = e.Attack(_player);
                if (enemyDmg < 0)
                {
                    missCount++;
                }
                else
                {
                    hitCount++;
                    totalDamage += enemyDmg;
                }
                continue;
            }
            
            if (dist <= 3) // enemy field vision to chase
            {
                // pass a predicate so the enemy only moves onto walkable, unoccupied tiles
                e.Chase(_player, pos => _walkables.Contains(pos) && !IsTileOccupied(pos));
            } 
            else
            {
                // random wander step
                var dirs = new[] { Vector2.N, Vector2.S, Vector2.E, Vector2.W };
                var step = dirs[Random.Shared.Next(dirs.Length)];
                var target = e.Pos + step;

                if (_walkables.Contains(target) && !IsTileOccupied(target))
                    e.Pos = target;
            }
        }
        // Single message for whole enemy phase
        if (hitCount > 0)
        {
            PushMessage($"Enemies hit you {hitCount}x for {totalDamage}.");
        }
        else if (missCount > 0)
        {
            PushMessage($"Enemies missed {missCount}x.");
        }
        updateDiscovered();
    }

    public static bool isWinner = false;
    public override void Update() {
        _player!.Update();

        EnemyPov();
        updateDiscovered();
        CheckWinCondition();

        // check for player death -- show GAME OVER screen and end level
        if (_player.Hp <= 0)
        {
            _levelActive = false;
            isWinner = false;
        }
        // foreach item update
        // foreach NPC update 
    }

   public override void Draw(IRenderWindow? disp) {
      // using custom RenderWindow, cast to my RenderWindow
      var tilesToDraw = new TileSet(_floor);
      tilesToDraw.UnionWith(_tunnel);
      tilesToDraw.UnionWith(_door);
      tilesToDraw.UnionWith(_decor);
      
      tilesToDraw.IntersectWith(_discovered);
      tilesToDraw.UnionWith(_inFov);

      disp.fDraw(tilesToDraw, _map, ConsoleColor.Gray);
      
      drawItems(disp);
      drawEnemies(disp);
      _player!.Draw(disp);
      disp.Draw(_player.HUD, new Vector2(0, 24), ConsoleColor.Green);
      
      // PROMPT: How to Display 2 MessageQueues on the same line
      var enemyMsg = _messages.Count > 0 ? _messages.Last() : "";
      var playerMsg = _lastPlayerCombatMessage;

      string line;
      if (string.IsNullOrWhiteSpace(enemyMsg))
          line = playerMsg;
      else if (string.IsNullOrWhiteSpace(playerMsg))
          line = enemyMsg;
      else
          line = $"{enemyMsg} | {playerMsg}";

      disp.Draw(line.PadRight(78), new Vector2(0, 23), ConsoleColor.Yellow);

      
   }

   public override void DoCommand(Command command) {
      // player ctl  
      if (command.Name == "up") {
         MovePlayer(Vector2.N);
      } else if (command.Name == "down") {
         MovePlayer(Vector2.S);
      } else if (command.Name == "left") {
         MovePlayer(Vector2.W);
      } else if (command.Name == "right") {
         MovePlayer(Vector2.E);
      } // game ctl      
      else if (command.Name == "quit") {
         _levelActive = false;
      } else if (command.Name == "drink-first-potion")
      {
          _player!.DrinkFirstPotion();
      } else if (command.Name == "cycle-weapon")
      {
          PushMessage(_player!.CycleWeapon()
              ? $"Equipped weapon: {_player.EquippedWeaponName} (+{_player.EquippedWeaponDamage})."
              : "No weapons to equip.");
      } else if (command.Name == "cycle-armour")
      {
          PushMessage(_player!.CycleArmour()
              ? $"Equipped armour: {_player.EquippedArmourName} (+{_player.EquippedArmourDefense})."
              : "No armour to equip.");
      }
   }

// -------------------------------------------------------------------------
    // Draws Items
   private void drawItems(IRenderWindow disp)
   {
      foreach (var i in _items)
      {
         if (_inFov.Contains(i.Pos) || _discovered.Contains(i.Pos)) i.Draw(disp);
      }
   }
    // Draws Enemies
   private void drawEnemies(IRenderWindow disp) 
    {
         foreach (var e in _enemies)
         {
            if (_inFov.Contains(e.Pos) || _discovered.Contains(e.Pos)) e.Draw(disp); 
         }
    }

   private void initMapTileSets(string map) {
      var lines = map.Split('\n');

      // ------ rules for map ------
      // . - floor, walkable and transparent.
      // + - door, walkable and transparent // # - tunnel, walkable and transparent
      // ' ' - solid stone, not walkable, not transparent.
      // '|' - wall, not walkable, not transparent, but discoverable.'
      //  others are treated the same as wall.
      // tunnel, wall, and doorways are decor, once discovered they are visible.

      _floor  = new TileSet();
      _tunnel = new TileSet();
      _door   = new TileSet();
      _decor  = new TileSet();

      foreach (var (c, p) in Vector2.Parse(map)) {
         if (c == '.') _floor.Add(p);
         else if (c == '+') _door.Add(p);
         else if (c == '#') _tunnel.Add(p);
         else if (c != ' ') _decor.Add(p);
      }

      _walkables = _floor.Union(_tunnel).Union(_door).ToHashSet();

//      for (int row = 0; row < lines.Length; ++row) {
//         for (int col = 0; col < lines[row].Length; ++col) {
//            char tile = lines[row][col];
//
//            if (tile == '.' || tile == '+' || tile == '#') {
//               _walkables.Add(new Vector2(col, row));
//               _decor.Add(new Vector2(col, row));
//            } else if (tile != ' ') {
//               _decor.Add(new Vector2(col, row));
//            }
//         }
//      }
   }

// ------------------------------------------------------
// Commands 
// ------------------------------------------------------

   
   private void registerCommandsWithScene() {
      RegisterCommand(ConsoleKey.UpArrow, "up");
      RegisterCommand(ConsoleKey.W, "up");
      RegisterCommand(ConsoleKey.K, "up");

      RegisterCommand(ConsoleKey.DownArrow, "down");
      RegisterCommand(ConsoleKey.S, "down");
      RegisterCommand(ConsoleKey.J, "down");

      RegisterCommand(ConsoleKey.LeftArrow, "left");
      RegisterCommand(ConsoleKey.A, "left");
      RegisterCommand(ConsoleKey.H, "left");

      RegisterCommand(ConsoleKey.RightArrow, "right");
      RegisterCommand(ConsoleKey.D, "right");
      RegisterCommand(ConsoleKey.L, "right");
      
      RegisterCommand(ConsoleKey.I, "cycle-weapon");
      RegisterCommand(ConsoleKey.P, "drink-first-potion");
      RegisterCommand(ConsoleKey.O, "cycle-armour");
      
      RegisterCommand(ConsoleKey.Q, "quit");
   }
   
   // PROMPT: I need a way to implement a Message buffer that doesn't cause Buffer corruption
   private readonly Queue<string> _messages = new();
   private const int MaxMessages = 3;
   private void PushMessage(string message)
   {
       if (string.IsNullOrWhiteSpace(message)) return;

       _messages.Enqueue(message);
       while (_messages.Count > MaxMessages)
       {
           _messages.Dequeue();
       }
   }
    // ALSO RETURNED FROM THE ABOVE PROMPT
   private static string ItemLabel(Item item)
   {
       return item switch
       {
           Weapon w => w.Name,
           Armour a => a.Name,
           Gold => "Gold",
           _ => item.GetType().Name
       };
   }
   
   
   private string _lastPlayerCombatMessage = "";
   // Helper method to check if the player has explored the entire map
   private bool HasExploredEntireMap()
   {
       var discoverable = _floor
           .Union(_tunnel)
           .Union(_door)
           .Union(_decor)
           .ToHashSet();

       return _discovered is not null && _discovered.IsSupersetOf(discoverable);
   }
   // Helper method to check for a win condition
   private void CheckWinCondition()
   {
       if (_enemies.Count == 0 && HasExploredEntireMap())
       {
           isWinner = true;
           QuitLevel();
       }
   }
   
   public void MovePlayer(Vector2 delta) {
        var newPos = _player!.Pos + delta;
        var enemy = _enemies.FirstOrDefault(e => e.Pos == newPos && e.Hp > 0);
        if (enemy != null)
        {
            int playerDmg = _player.Attack(enemy);
            _lastPlayerCombatMessage = playerDmg <= 0
                ? $"You miss {enemy.EnemyType}."
                : $"You hit {enemy.EnemyType} for {playerDmg}.";
    
            PushMessage(_lastPlayerCombatMessage);

            // If enemy died, reward, push message and end here. 
            if (enemy.Hp <= 0)
            {
                _player.AddExp(enemy.ExpValue);
                _enemies.Remove(enemy);
                PushMessage($"{enemy.EnemyType} is defeated.");
            }
            CheckWinCondition();
            return;
        }

        if (_walkables.Contains(newPos))
        {
            var itemToPickUp = _items.FirstOrDefault(i => i.Pos == newPos);

            if (itemToPickUp != null)
            {
                if (itemToPickUp is Gold g)
                {
                    _player.AddGold(g.Amount);
                    _items.Remove(itemToPickUp);
                    PushMessage($"You pick up {g.Amount} gold.");
                }
                else
                {
                    if (_player.Inventory.AddItem(itemToPickUp))
                    {
                        bool autoEquipped = _player.TryAutoPickup(itemToPickUp);
                        _items.Remove(itemToPickUp);

                        PushMessage($"You pick up {ItemLabel(itemToPickUp)}.");

                        if (autoEquipped)
                            PushMessage($"{ItemLabel(itemToPickUp)} auto-equipped.");
                    }
                }
            }

            _player!.Pos = newPos;
        } 
   }

    // I need a way for enemy Glyphs not be walkable and needs to stay on screen until hp is 0
    // checks if there is an enemy already on the tiles.
    private bool IsTileOccupied(Vector2 pos)
    {
        if (_player.Pos == pos)
            return true;

        return _enemies.Any(e => e.Pos == pos && e.Hp > 0);
    }

    public void QuitLevel() {
      _levelActive = false;
   }
}