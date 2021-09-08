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
        if (Input.GetMouseButtonDown(0))
        {
            //if (clicked_char || clicked_ui)
            {
                // 클릭 취소
                if (MoveButtons.nav_on)
                {
                    // 플레이어 이동범위 표시한거 지우고 클릭이 유효하면 해당위치로 이동
                    Vector3Int gridMousePos = GetGridPos();
                    StageManager.stageManager.mapManager.tilemaps[2].SetTile(gridMousePos, StageManager.stageManager.mapManager.nav_tile);
                    // tilemap 범위가 맵 밖인경우
                    int x = gridMousePos.x;
                    int y = gridMousePos.y;
                    if (x >= 5 && x < 5 + Constants.mapHeight && y >= 5 && y < 5 + Constants.mapWidth)
                    {
                        if (StageManager.stageManager.mapManager.tilemaps[2].GetTile(gridMousePos) != null)
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

                }
                //if (clicked_ui)
                {
                    //clicked_ui = false;
                    //켜져있는 ui 닫기 or ui 클릭했는지 확인
                }
                return;
            }
            // else
            // {
            //     Vector2 worldpoint = camera.ScreenToWorldPoint(Input.mousePosition);
            //     RaycastHit2D hit2D = Physics2D.Raycast(worldpoint, Vector2.zero);
            //     if (hit2D.collider != null)
            //     {
            //         //Debug.Log(hit2D.collider.name);
            //         Characters click_target = hit2D.collider.GetComponent<Characters>();
            //         if (click_target != null)
            //         {
            //             // 적 or 플레이어 클릭 
            //             clicked_char = true;
            //             click_target.onclick();
            //         }
            //     }
            //     else
            //     {
            //         // tile 정보 표시? 이건 내일 물어봐야지
            //     }
            // }
        }
    }

    Vector3Int GetGridPos()
    {
        Vector3 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
        return StageManager.stageManager.mapManager.tilemaps[0].WorldToCell(mouseWorldPos);
    }
}
