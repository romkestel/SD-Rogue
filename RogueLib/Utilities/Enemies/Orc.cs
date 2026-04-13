using RogueLib.Dungeon;
using System;
using System.Collections.Generic;
using System.Text;

namespace RogueLib.Utilities.Enemies;

public class Orc : Enemy
{
    public Orc(Vector2 pos, int damage) : base('O', pos)
    {

    }

    public override void Attack(Player _player)
    {
        Console.WriteLine("Orc swings a big Club!");
    }


    public override void Draw(IRenderWindow disp)
    {
        disp.Draw(Glyph, Pos, ConsoleColor.Red);
    }
}
