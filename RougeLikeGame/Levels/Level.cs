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
   protected int     _senseRadius = 400;
   
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
        SpawnEnemey();
        SpreadItem();

        // Spawns Items
        void SpreadItem()
      {
         var rng = new Random();
         var am = rng.Next(10, 20);
         
         var wep = rng.Next(0, 3);
         var armour = rng.Next(0, 2);
         var hPotion = rng.Next(0, 2);
         var strPotion = rng.Next(0, 2);
         

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
        void SpawnEnemey()
        {
            var rng = new Random();
            var am = rng.Next(10, 20);
            var enemy = rng.Next(1, 3);

            for (int i = 0; i < enemy; i++)
            {
                var tile = _floor.ElementAt(rng.Next(_floor.Count));
                _enemies.Add(new Goblin (tile, 5));
            }
            for (int i = 0; i < enemy; i++)
            {
                var tile = _floor.ElementAt(rng.Next(_floor.Count));
                _enemies.Add(new Orc(tile, 10));
            }
            for (int i = 0; i < enemy; i++)
            {
                var tile = _floor.ElementAt(rng.Next(_floor.Count));
                _enemies.Add(new Troll(tile, 15));
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
    

   public void EnemyPov()
    {
      foreach (var e in _enemies)
        {
            // I need a method for the enemy to chase the player when
            // within enemy radius BUT it needs to only chase the
            // player WITHIN walkable tiles
            var enemyFov = (e.Pos - _player.Pos).RookLength; //fovCalc(e.Pos, _senseRadius);
                        
            if (enemyFov < 10)
            {
                IsTileOccupied(_player.Pos);
                IsTileOccupied(e.Pos);
                e.Chase(_player);
            }                
        }
        updateDiscovered();
    }

    public override void Update() {
      _player!.Update();

        EnemyPov();
        updateDiscovered();
        // foreach item update
        // foreach NPC update 
        // check for player death -- on death build RIP message
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

      var rng = new Random();
      if (_player.Turn % 5 == 0)
         _player._color = (ConsoleColor)rng.Next(10, 16);
      
      // disp.Draw(_player!.Glyph, _player!.Pos, ConsoleColor.Cyan);

      drawItems(disp);
      drawEnemies(disp);
      _player!.Draw(disp);
      disp.Draw(_player.HUD, new Vector2(0, 23), ConsoleColor.Green);
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
          _player!.CycleWeapon();
      } else if (command.Name == "cycle-armour")
      {
          _player!.CycleArmour();
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

   
   public void MovePlayer(Vector2 delta) {
      var newPos = _player!.Pos + delta;
        var enemy = _enemies.FirstOrDefault(e => e.Pos == newPos && e._hp > 0);
        if (enemy != null) 
        { 
            _player.Attack(enemy);
            enemy.Attack(_player);

            if (enemy._hp <= 0)
            {
                _enemies.Remove(enemy);
            }

            return;
        }

        if (_walkables.Contains(newPos)) {
            var itemToPickUp = _items.FirstOrDefault(i => i.Pos == newPos);
            
            if (itemToPickUp != null)
            {
                if (itemToPickUp is Gold g)
                {
                    _player.AddGold(g.Amount);
                    _items.Remove(itemToPickUp);
                }
                else
                {
                    if (_player.Inventory.AddItem(itemToPickUp))
                    {
                        _player.TryAutoPickup(itemToPickUp);
                        _items.Remove(itemToPickUp);
                    }
                }
            }

            var oldPos = _player!.Pos;
            _player!.Pos = newPos;
            //_walkables.Remove(newPos); // new tile is now occupied
            //_walkables.Add(oldPos);    // old tile is now free
                        
        } 
   }

    // I need a way for enemy Glyphs not be walkable and needs to stay on screen until hp is 0
    // checks if there is an enemy already on the tiles.
    private bool IsTileOccupied(Vector2 pos)
    {
        if (_player.Pos == pos)
            return true;

        return _enemies.Any(e => e.Pos == pos && e._hp > 0);
    }

    //public void MoveEnemy(Vector2 delta)
    //{
    //    var dy = delta.Y - _enemies.Pos.Y;

    //    var newPos = _enemies!.Pos + delta;

    //    // Move from MovePlayer Method
    //    if (_walkables.Contains(newPos) || IsTileOccupied(newPos))
    //    {
    //        var oldPos = _enemies.Pos;
    //        _enemies.Pos = newPos;
         
    //    }

    //    updateDiscovered();
    //}

    public void QuitLevel() {
      _levelActive = false;
   }



}