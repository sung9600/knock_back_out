using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtk : Phases
{
    Enemy[] enemies;
    public override void OnStartPhase()
    {
        enemies = FindObjectsOfType<Enemy>();
        TurnManager.turnManager.indicator.SetText("enemy atk phase");
        TurnManager.turnManager.phase = phase.enemy_atk;
    }
    public override bool IsComplete()
    {
        foreach (Enemy enemy in enemies)
        {
            if (!enemy.turn_done)
            {
                enemy.Attack_animation();
                return false;
            }
        }
        int i = 1;
        while (i > 0)
        {
            int x = Random.Range(0, Constants.mapHeight);
            int y = Random.Range(0, Constants.mapHeight);
            if (!MapManager.checkCantGoTile(x, y, true))
            {
                int count = StageManager.stageManager.getEnemy_Prefabs().Count;
                int index = Random.Range(0, count);
                GameObject go = Instantiate(StageManager.stageManager.getEnemy_Prefab_byIndex(index)
                    , Constants.character_tile_offset + MapManager.mapManager.GetTilemap(0).GetCellCenterWorld(new Vector3Int(x, y, 0)), Quaternion.identity);
                go.GetComponent<Enemy>().init(new Pos(x, y));
                i--;
            }
        }
        enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
            enemy.turn_done = false;
        return true;
    }
}
