using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommand
{
    public Pos from, to;
    public Characters Attacker;


    public AttackCommand(Pos Ifrom, Pos Ito, Characters who)
    {
        from = Ifrom;
        to = Ito;
        Attacker = who;
    }

    public void Execute()
    {
        switch (Attacker.stat.atkType)
        {
            case atkType.melee:
                attack_melee();
                break;
            case atkType.direct:
                attack_direct();
                break;
            case atkType.indirect:
                attack_indirect();
                break;
        }
    }

    public void attack_melee()
    {
        Pos attacked_position = Attacker.curpos + (to - from);
        Characters target = StageManager.stageManager.GetCharacterByPos(attacked_position);
        if (target != null)
        {
            target.GetHit();
        }
        Attacker.Attack_animation();
    }

    public void attack_direct()
    {
        Pos attack_dir = Pos.getDir(from, to);

        for (int i = 1; i < Constants.mapWidth; i++)
        {
            Pos p = Attacker.curpos + i * attack_dir;
            if (!MapManager.checkCantGoTile(p, true))
            {
                Characters target = StageManager.stageManager.GetCharacterByPos(p);
                if (target != null)
                {
                    target.GetHit();
                }
                break;
            }

        }
        Attacker.Attack_animation();

    }
    public void attack_indirect()
    {

    }
}

public class AttackList
{
    public List<AttackCommand> attackList;

    public void Reset()
    {
        attackList.Clear();
    }

    public AttackList()
    {
        attackList = new List<AttackCommand>();
    }
}
