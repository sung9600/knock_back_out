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

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Vector3Int pos = new Vector3Int(i, j, 0);
                if (MapManager.mapManager.GetTilemap(3).GetTile(pos) == MapManager.mapManager.GetTile(0, 8))
                {
                    MapManager.mapManager.GetTilemap(3).SetTile(pos, null);

                    int count = StageManager.stageManager.getEnemy_Prefabs().Count;
                    int index = Random.Range(0, count);
                    GameObject go = Instantiate(StageManager.stageManager.getEnemy_Prefab_byIndex(index)
                        , Constants.character_tile_offset + MapManager.mapManager.GetTilemap(0).GetCellCenterWorld(pos), Quaternion.identity);
                    go.GetComponent<Enemy>().init(new Pos(i, j));
                    i--;
                    return false;
                }
            }

        }
        enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
            enemy.turn_done = false;
        return true;
    }
}
