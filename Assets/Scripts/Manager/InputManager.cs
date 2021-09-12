using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputManager
{
    private Camera camera;
    //private static bool clicked_char = false;
    //private static bool clicked_ui = false;

    public void init()
    {
        camera = Camera.main;
    }

    public void onUpdate()
    {
        Debug.Log(Cards.clicked_card);
        if (Input.GetMouseButtonDown(0))
        {
            if (!Cards.usingcard)
            {
                // 이동클릭
                if (MoveButtons.nav_on && StageManager.stageManager.player.status == Character_status.waiting)
                {
                    // 플레이어 이동범위 표시한거 지우고 클릭이 유효하면 해당위치로 이동
                    Vector3Int gridMousePos = GetGridPos();
                    // tilemap 범위가 맵 밖인경우
                    int x = gridMousePos.x;
                    int y = gridMousePos.y;
                    gridMousePos.z = 0;
                    if (x >= 5 && x < 5 + Constants.mapHeight && y >= 5 && y < 5 + Constants.mapWidth)
                    {
                        if (StageManager.stageManager.mapManager.tilemaps[2].GetTile(gridMousePos - new Vector3Int(5, 5, 0)) != null)
                        {
                            List<Pos> path = StageManager.stageManager.mapManager
                                .getPath(StageManager.stageManager.player.curpos.x, StageManager.stageManager.player.curpos.y, x - 5, y - 5);
                            if (path != null)
                            {
                                StageManager.stageManager.player.move(path);
                                StageManager.stageManager.player.curpos.x = x - 5;
                                StageManager.stageManager.player.curpos.y = y - 5;
                            }
                        }
                    }
                    StageManager.stageManager.mapManager.clearNavTiles();
                    MoveButtons.nav_on = false;
                }
            }
            else
            {
                // 카드 공격클릭
                Vector3Int gridMousePos = GetGridPos();
                // tilemap 범위가 맵 밖인경우
                int x = gridMousePos.x;
                int y = gridMousePos.y;
                gridMousePos.z = 0;
                if (StageManager.stageManager.mapManager.tilemaps[2].GetTile(gridMousePos - new Vector3Int(5, 5, 0)) != null)
                {
                    StageManager.stageManager.player.status = Character_status.attacking;
                    StageManager.stageManager.player.changeDir(StageManager.stageManager.player.curpos, new Pos(x - 5, y - 5));
                    StageManager.stageManager.mapManager.update_tileanims((x - 5) * 5 + (y - 5), 1);
                    Cards.usingcard = false;
                    Cards.clicked_card = -1;
                    StageManager.stageManager.mapManager.clearNavTiles();
                }
            }
        }
    }

    Vector3Int GetGridPos()
    {
        Vector3 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
        return StageManager.stageManager.mapManager.tilemaps[0].WorldToCell(mouseWorldPos);
    }
}
