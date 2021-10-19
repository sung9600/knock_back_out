using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : Phases
{
    Enemy[] enemies;
    public override void OnStartPhase()
    {
        enemies = FindObjectsOfType<Enemy>();
        TurnManager.turnManager.indicator.SetText("enemy move phase");
        TurnManager.turnManager.phase = phase.enemy_move;
    }
    public override bool IsComplete()
    {
        foreach (Enemy enemy in enemies)
        {
            if (!enemy.turn_done)
            {
                enemy.SetTarget();
                return false;
            }
        }
        enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
            enemy.turn_done = false;
        return true;
    }
}
