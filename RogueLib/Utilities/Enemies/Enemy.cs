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
    public int _hp = 5;
    public int _atk = 4;
    protected int _arm = 1;
    protected int _expvalue = 10;
    public int ExpValue => _expvalue;
    protected int _turn = 0;
    
    
    
    public Enemy(char glyph, Vector2 pos)
    {        
        Glyph = glyph;
        Pos = pos;
    }
        
    public virtual void Attack(Player player)
    {
        // Default attack behaviour: deal this enemy's attack value to the player.
        // Player.TakeDamage will account for the player's armour.
        player.TakeDamage(_atk);
    }

    public void TakeDamage(int damage)
    {
        int damageTaken = Math.Max(0, damage - _arm);
        _hp -= damageTaken;
    }

    //// intellicense
    //public void Chase(Player player)
    //{
    //    // Simple chasing logic: move towards the player
    //    if (player.Pos.X < (Pos.X+1))
    //        Pos.X--;

    //    if (player.Pos.X > (Pos.X-1))
    //        Pos.X++;

    //    if (player.Pos.Y < (Pos.Y+1))
    //        Pos.Y--;

    //    if (player.Pos.Y > (Pos.Y-1))
    //        Pos.Y++;
    //}

    // PROMPT : Show me a code where Enemies does only walks on walkable tiles
    public void Chase(Player player) => Chase(player, pos => true);

    // New: only move when canMove returns true (use Level to pass walkable + occupancy checks)
    public void Chase(Player player, Func<Vector2, bool> canMove)
    {
        // Basic greedy step towards player with checks
        var dx = Math.Sign(player.Pos.X - Pos.X);
        var dy = Math.Sign(player.Pos.Y - Pos.Y);

        // Try horizontal step first
        if (dx != 0)
        {
            var candidate = new Vector2(Pos.X + dx, Pos.Y);
            if (canMove(candidate))
            {
                Pos = candidate;
                return;
            }
        }
        // Try vertical step
        if (dy != 0)
        {
            var candidate = new Vector2(Pos.X, Pos.Y + dy);
            if (canMove(candidate))
            {
                Pos = candidate;
                return;
            }
        }
        // Try diagonal
        if (dx != 0 && dy != 0)
        {
            var candidate = new Vector2(Pos.X + dx, Pos.Y + dy);
            if (canMove(candidate))
            {
                Pos = candidate;
                return;
            }
        }
        // Fallback: try any adjacent cardinal tile
        var neighbors = new[]
        {
        new Vector2(Pos.X + 1, Pos.Y),
        new Vector2(Pos.X - 1, Pos.Y),
        new Vector2(Pos.X, Pos.Y + 1),
        new Vector2(Pos.X, Pos.Y - 1)
    };
        foreach (var n in neighbors)
        {
            if (canMove(n))
            {
                Pos = n;
                return;
            }
        }

        // If none available, stay in place
    }




    public virtual void Draw(IRenderWindow disp)
    {
        disp.Draw(Glyph, Pos, ConsoleColor.Red);
    }
}
