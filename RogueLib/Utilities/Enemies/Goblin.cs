using RogueLib.Dungeon;
using System;
using System.Collections.Generic;
using System.Text;

namespace RogueLib.Utilities.Enemies;

public class Goblin : Enemy
{
    public Goblin(Vector2 pos) : base('G', pos)
    {
        _hp = 6;
        _atk = 4;
        _expvalue = 8;
    }

    public override int Attack(Player? player)
    {
        return base.Attack(player);
    }
    

    public override void Draw(IRenderWindow disp)
    {
        disp.Draw(Glyph, Pos, ConsoleColor.Red);
    }
}
