using RogueLib.Dungeon;
using System;
using System.Collections.Generic;
using System.Text;

namespace RogueLib.Utilities.Enemies;

public class Troll : Enemy
{
    public Troll(Vector2 pos, int dmge) : base('T', pos)
    {

    }

    public override void Attack()
    {
        Console.WriteLine("Troll SMASH!");
    }


    public override void Draw(IRenderWindow disp)
    {
        disp.Draw(Glyph, Pos, ConsoleColor.Red);
    }
}
