using RogueLib.Dungeon;
using System;
using System.Collections.Generic;
using System.Text;

namespace RogueLib.Utilities.Enemies;

public class Orc : Enemy
{
    public Orc(Vector2 pos) : base('O', pos)
    {
        _hp = 10;
        _atk = 6;
        _expvalue = 12;
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
