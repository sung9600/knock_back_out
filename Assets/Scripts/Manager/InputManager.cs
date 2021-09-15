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
            Vector3Int gridMousePos = GetGridPos();
            int x = gridMousePos.x;
            int y = gridMousePos.y;
            gridMousePos.z = 0;
            if (!Cards.usingcard)
            {
                // 이동클릭
                if (MoveButtons.nav_on && StageManager.stageManager.player.status == Character_status.waiting)
                {
                    // 플레이어 이동범위 표시한거 지우고 클릭이 유효하면 해당위치로 이동
                    // tilemap 범위가 맵 밖인경우
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
                if (Player.usedCardInfo.cardInfo.type == CardType.Attack_direct)
                {
                    Debug.Log("direct");
                    if (StageManager.stageManager.mapManager.tilemaps[2].GetTile(gridMousePos - new Vector3Int(5, 5, 0)) != null)
                    {
                        Pos target = new Pos(x - 5, y - 5) - StageManager.stageManager.player.curpos;
                        bool up = target.x + target.y > 0;
                        bool left = target.x < 0 || (target.x == 0 && target.y > 0);
                        Debug.Log($"up:{up},left:{left}");
                        StageManager.stageManager.player.changeDir(StageManager.stageManager.player.curpos, new Pos(x - 5, y - 5));
                        for (int i = 1; i < Constants.mapHeight; i++)
                        {
                            int nx, ny;
                            if (left)
                            {
                                if (up)
                                {
                                    nx = StageManager.stageManager.player.curpos.x;
                                    ny = StageManager.stageManager.player.curpos.y + i;
                                }
                                else
                                {
                                    nx = StageManager.stageManager.player.curpos.x - i;
                                    ny = StageManager.stageManager.player.curpos.y;
                                }
                            }
                            else
                            {
                                if (up)
                                {
                                    nx = StageManager.stageManager.player.curpos.x + i;
                                    ny = StageManager.stageManager.player.curpos.y;
                                }
                                else
                                {
                                    nx = StageManager.stageManager.player.curpos.x;
                                    ny = StageManager.stageManager.player.curpos.y - i;
                                }
                            }
                            Debug.Log($"{nx},{ny}");
                            if (!MapManager.checkWidthHeight(nx, ny))
                            {
                                // todo : 끝에 도달해서 총알 튀는 애니메이션
                                break;
                            }
                            if (StageManager.stageManager.mapManager.map[ny * 5 + nx] == (int)tileType.rock)
                            {
                                Debug.Log($"rock at{nx}.{ny}");
                                break;
                            }
                        }

                    }
                }
                else
                {
                    Debug.Log("direct else");
                    // 카드 공격클릭(근접,곡사)
                    // tilemap 범위가 맵 밖인경우
                    if (StageManager.stageManager.mapManager.tilemaps[2].GetTile(gridMousePos - new Vector3Int(5, 5, 0)) != null)
                    {
                        StageManager.stageManager.player.changeDir(StageManager.stageManager.player.curpos, new Pos(x - 5, y - 5));
                        StageManager.stageManager.player.status = Character_status.attacking;
                        StageManager.stageManager.mapManager.update_tileanims((x - 5) * 5 + (y - 5), 1);
                        Cards.usingcard = false;
                        Cards.clicked_card = -1;
                        StageManager.stageManager.mapManager.clearNavTiles();
                    }
                }
                StageManager.stageManager.mapManager.clearNavTiles();
                MoveButtons.nav_on = false;
                Player.usedCardInfo = null;
            }
        }
    }

    Vector3Int GetGridPos()
    {
        Vector3 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
        return StageManager.stageManager.mapManager.tilemaps[0].WorldToCell(mouseWorldPos);
    }
}
