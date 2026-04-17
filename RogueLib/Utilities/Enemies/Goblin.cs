using RogueLib.Dungeon;
using System;
using System.Collections.Generic;
using System.Text;

namespace RogueLib.Utilities.Enemies;

public class Goblin : Enemy
{
    public Goblin(Vector2 pos, int damage) : base('G', pos)
    {
        _atk = damage;
    }

    public override void Attack(Player? player)
    {
        Console.WriteLine("Goblin attacks!");
        base.Attack(player);
    }
    

    public override void Draw(IRenderWindow disp)
    {
        disp.Draw(Glyph, Pos, ConsoleColor.Red);
    }
}
