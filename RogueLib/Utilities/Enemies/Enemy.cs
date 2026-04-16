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

    // intellicense
    public void Chase(Player player)
    {
        // Simple chasing logic: move towards the player
        if (player.Pos.X < (Pos.X+1))
            Pos.X--;

        if (player.Pos.X > (Pos.X-1))
            Pos.X++;

        if (player.Pos.Y < (Pos.Y+1))
            Pos.Y--;

        if (player.Pos.Y > (Pos.Y-1))
            Pos.Y++;
    }



    public virtual void Draw(IRenderWindow disp)
    {
        disp.Draw(Glyph, Pos, ConsoleColor.Red);
    }
}
