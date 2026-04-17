using RogueLib.Dungeon;
using System;
using System.Collections.Generic;
using System.Text;

namespace RogueLib.Utilities.Enemies;

public class Troll : Enemy
{
    
    public Troll(Vector2 pos) : base('T', pos)
    {
        _atk = 8;
        _hp = 14;
        _expvalue = 18;
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
