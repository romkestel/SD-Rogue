using RogueLib.Dungeon;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace RogueLib.Utilities.Enemies;
public abstract class Enemy : IActor, IDrawable
{
    public char Glyph { get; init; }
    public Vector2 Pos;
    public ConsoleColor _color = ConsoleColor.Red;
        
    protected int _level = 1;
    protected int _hp = 7;
    protected int _atk = 4;
    protected int _arm = 2;
    protected int _expvalue = 10;
    public int ExpValue => _expvalue;
    protected int _turn = 0;
    public int Hp => _hp;
    public int Atk => _atk;
    public int Arm => _arm;
    public bool IsDead => _hp <= 0;

    public string EnemyType => GetType().Name;
    
    public Enemy(char glyph, Vector2 pos)
    {        
        Glyph = glyph;
        Pos = pos;
    }
        
    public virtual int Attack(Player? player)
    {
        if (player is null) return -1;
        
        // d20 dice roll for hit/miss chance
        int toHit = Dice.Roll(20);
        if (toHit <= 6) return -1; // miss
        
        // d_ dice roll depending on enemy _atk amount
        int damage = Dice.Roll(1, Math.Max(1, _atk)) + Math.Max(1, _atk / 3);
        return player.TakeDamage(Math.Max(1, damage));
    }

    public virtual int TakeDamage(int damage)
    {
        int damageTaken = Math.Max(0, damage - _arm);
        _hp -= damageTaken;
        return damageTaken;
    }

    

    // PROMPT : Show me a code where Enemies does only walks on walkable tiles
    public virtual void Chase(Player player) => Chase(player, pos => true);

    // PROMPT: Show me a good way to stop enemies from running away instead of chasing
    public virtual void Chase(Player player, Func<Vector2, bool> canMove)
    {
        var dx = Math.Sign(player.Pos.X - Pos.X);
        var dy = Math.Sign(player.Pos.Y - Pos.Y);
        
        // uses KingLength for square range
        var currentDist = (player.Pos - Pos).KingLength;

        var candidates = new[]
        {
            new Vector2(Pos.X + dx, Pos.Y),         // horizontal
            new Vector2(Pos.X, Pos.Y + dy),         // vertical
            new Vector2(Pos.X + dx, Pos.Y + dy)     // diagonal
        };

        foreach (var c in candidates)
        {
            if (c == Pos) continue;
            if (!canMove(c)) continue;

            var newDist = (player.Pos - c).KingLength;
            if (newDist < currentDist)
            {
                Pos = c;
                return;
            } 
        }

        // no valid closer tile -> stay in place
    }
    

    public virtual void Draw(IRenderWindow disp)
    {
        disp.Draw(Glyph, Pos, ConsoleColor.Red);
    }
}
