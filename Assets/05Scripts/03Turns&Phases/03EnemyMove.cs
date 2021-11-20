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
        int i = 1;
        while (i > 0)
        {
            int idx = Random.Range(0, 16);
            int[] x = { 0, 1, 2, 3, 4, 0, 4, 0, 4, 0, 4, 0, 1, 2, 3, 4 };
            int[] y = { 0, 0, 0, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 4, 4, 4 };
            Pos pos = new Pos(x[idx], y[idx]);
            if (!MapManager.checkCantGoTile(pos.x, pos.y, true))
            {
                //Debug.Log("warning at " + pos);
                MapManager.mapManager.GetTilemap(2).SetTile(new Vector3Int(pos.x, pos.y, 0), MapManager.mapManager.GetTile(0, 7));
                //Debug.Log(MapManager.mapManager.GetTilemap(2).GetTile(new Vector3Int(pos.x, pos.y, 0)));
                i--;
            }
            // int x = Random.Range(0, Constants.mapHeight);
            // int y = Random.Range(0, Constants.mapHeight);
            // if (!MapManager.checkCantGoTile(x, y, true))
            // {
            // }
        }
        enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
            enemy.turn_done = false;
        return true;
    }
}
