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
        // foreach (Enemy enemy in enemies)
        // {
        //     if (!enemy.turn_done)
        //     {
        //         enemy.Attack_animation();
        //         return false;
        //     }
        // }

        foreach (AttackCommand atkc in StageManager.stageManager.attackList.attackList)
        {
            // 누가 공격했는지 찾고
            if (atkc.Attacker != null)
            {
                // 그 사람이 atk dir에 현재위치 더한곳 공격
                atkc.Attacker.Attack_animation();
                StageManager.stageManager.attackList.attackList.Remove(atkc);
                return false;
            }
        }


        for (int i = 0; i < Constants.mapHeight; i++)
        {
            for (int j = 0; j < Constants.mapHeight; j++)
            {
                Vector3Int pos = new Vector3Int(i, j, 0);
                if (MapManager.mapManager.GetTilemap(2).GetTile(pos) == MapManager.mapManager.GetTile(0, 7))
                {
                    MapManager.mapManager.GetTilemap(2).SetTile(pos, null);

                    int count = StageManager.stageManager.getEnemy_Prefabs().Count;
                    int index = Random.Range(0, count);
                    GameObject go = Instantiate(StageManager.stageManager.getEnemy_Prefab_byIndex(index)
                        //, Constants.character_tile_offset + MapManager.mapManager.GetTilemap(0).GetCellCenterWorld(pos), Quaternion.identity
                        , StageManager.stageManager.getCharacterCanvas());
                    go.GetComponent<RectTransform>().anchoredPosition = Constants.character_tile_offset + Camera.main.WorldToScreenPoint(MapManager.mapManager.GetTilemap(0).GetCellCenterWorld(pos));
                    go.GetComponent<Enemy>().init(new Pos(i, j));
                    go.name = StageManager.stageManager.getEnemy_Prefab_byIndex(index).name + StageManager.stageManager.character_count;
                    StageManager.stageManager.character_count++;
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
