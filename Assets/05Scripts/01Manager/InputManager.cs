using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

[System.Serializable]
public class InputManager
{
    private Camera camera;

    [SerializeField]
    private CardsPanel cardsPanel;
    //private static bool clicked_char = false;
    //private static bool clicked_ui = false;

    public void init()
    {
        camera = Camera.main;
        cardsPanel = GameObject.FindObjectOfType<CardsPanel>();
    }

    private Vector3 First_Touch_Position;
    private Vector3 Last_Touch_Position;

    private Vector3Int Last_Touched_Grid = new Vector3Int(-100, -100, -100);
    private Vector3Int default_Grid = new Vector3Int(-100, -100, -100);

    private bool line_on = false;
    public void onLateUpdate()
    {
        #region 기존
        // if (Input.GetMouseButtonDown(0))
        // {
        //     // ui click 예외처리
        //     if (EventSystem.current.IsPointerOverGameObject())
        //     {
        //         Debug.Log("ui click");
        //         return;
        //     }
        //     if (TurnManager.turnManager.phase != phase.player_turn)
        //     {
        //         Debug.Log("not player turn");
        //         return;
        //     }
        //     Vector3Int gridMousePos = GetGridPos();
        //     int x = gridMousePos.x;
        //     int y = gridMousePos.y;
        //     gridMousePos.z = 0;
        //     Player player = StageManager.stageManager.GetPlayer();
        //     if (!CardUI.usingcard)
        //     {
        //         // 이동클릭
        //         Debug.Log("input for moving");
        //         // 5x5그리드 안 클릭인경우
        //         if (x >= 5 && x < 5 + Constants.mapHeight && y >= 5 && y < 5 + Constants.mapWidth)
        //         {
        //             // 이동버튼 눌렀고, 이동중 아닌경우
        //             if (MoveButtons.nav_on && player.status == Character_status.waiting)
        //             {
        //                 // 플레이어 이동범위 표시한거 지우고 클릭이 유효하면 해당위치로 이동
        //                 // tilemap 범위가 맵 밖인경우
        //                 if (MapManager.mapManager.GetTilemap(1).GetTile(gridMousePos - new Vector3Int(5, 5, 0)) != null)
        //                 {
        //                     List<Pos> path = MapManager.getPath(player.curpos.x, player.curpos.y, x - 5, y - 5, player.stat.moveType == moveType.ground);
        //                     if (path != null)
        //                     {
        //                         player.remain_move -= path.Count - 1;
        //                         player.move(path);
        //                         MapManager.mapManager.map[player.curpos.x, player.curpos.y] &= 0x0000FFFF;

        //                         player.curpos.x = x - 5;
        //                         player.curpos.y = y - 5;
        //                         MapManager.mapManager.map[x - 5, y - 5] |= 0x00010000;
        //                     }
        //                 }
        //             }
        //             MapManager.mapManager.clearNavTiles();
        //             MoveButtons.nav_on = false;
        //         }

        //     }
        //     else if (player.usedcard.cardInfo.type != CardType.end)
        //     {
        //         switch (player.usedcard.cardInfo.type)
        //         {
        //             case CardType.Attack_direct:
        //                 // 직사공격
        //                 if (MapManager.mapManager.GetTilemap(1).GetTile(gridMousePos - new Vector3Int(5, 5, 0)) != null)
        //                 {
        //                     Pos direction = new Pos(x - 5, y - 5) - player.curpos;
        //                     bool up = direction.x + direction.y > 0;
        //                     bool left = direction.x < 0 || (direction.x == 0 && direction.y > 0);
        //                     if (!left) player.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        //                     for (int i = 1; i < Constants.mapHeight; i++)
        //                     {
        //                         int nx, ny;
        //                         if (left)
        //                         {
        //                             if (up)
        //                             {
        //                                 nx = player.curpos.x;
        //                                 ny = player.curpos.y + i;
        //                             }
        //                             else
        //                             {
        //                                 nx = player.curpos.x - i;
        //                                 ny = player.curpos.y;
        //                             }
        //                         }
        //                         else
        //                         {
        //                             if (up)
        //                             {
        //                                 nx = player.curpos.x + i;
        //                                 ny = player.curpos.y;
        //                             }
        //                             else
        //                             {
        //                                 nx = player.curpos.x;
        //                                 ny = player.curpos.y - i;
        //                             }
        //                         }
        //                         // todo : nx,ny에 적있는지 체크
        //                         if (!MapManager.checkWidthHeight(nx, ny))
        //                         {
        //                             // todo : 끝에 도달해서 총알 튀는 애니메이션
        //                             break;
        //                         }
        //                         if (MapManager.mapManager.map[nx, ny] == (int)tileType.rock)
        //                         {
        //                             break;
        //                         }
        //                         MapManager.mapManager.update_tileanims(nx, ny, 1);
        //                     }
        //                     CardUI.usingcard = false;
        //                     CardUI.clicked_card = -1;
        //                     MapManager.mapManager.clearNavTiles();
        //                     MoveButtons.nav_on = false;
        //                     player.usedcard.cardInfo.type = CardType.end;
        //                 }
        //                 break;
        //             case CardType.Attack_indirect:
        //                 // 곡사공격
        //                 if (MapManager.mapManager.GetTilemap(1).GetTile(gridMousePos - new Vector3Int(5, 5, 0)) != null)
        //                 {
        //                     Pos direction = new Pos(x - 5, y - 5) - player.curpos;
        //                     bool left = direction.x < 0 || (direction.x == 0 && direction.y > 0);
        //                     if (!left) player.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        //                     player.status = Character_status.attacking;
        //                     // Todo : animation
        //                     player.GetAnimator().SetTrigger("Shoot");
        //                     GameObject bullet = GameObject.Instantiate(player.bullet_prefab, player.transform.position, Quaternion.identity, player.transform);
        //                     bullet.AddComponent<parabola>();
        //                     bullet.GetComponent<parabola>().shoot(new Vector3Int(gridMousePos.x - 5, gridMousePos.y - 5, 0));
        //                     //StageManager.stageManager.mapManager.update_tileanims((x - 5) * 5 + (y - 5), 1);
        //                     CardUI.usingcard = false;
        //                     CardUI.clicked_card = -1;
        //                     MapManager.mapManager.clearNavTiles();
        //                     MoveButtons.nav_on = false;
        //                     player.usedcard.cardInfo.type = CardType.end;
        //                 }
        //                 break;
        //             case CardType.Attack_near:
        //                 // 근접공격
        //                 if (MapManager.mapManager.GetTilemap(1).GetTile(gridMousePos - new Vector3Int(5, 5, 0)) != null)
        //                 {
        //                     Pos direction = new Pos(x - 5, y - 5) - player.curpos;
        //                     bool left = direction.x < 0 || (direction.x == 0 && direction.y > 0);
        //                     if (!left) player.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        //                     player.status = Character_status.attacking;
        //                     player.GetAnimator().SetTrigger("Attack");
        //                     MapManager.mapManager.update_tileanims((x - 5), (y - 5), 1);
        //                     CardUI.usingcard = false;
        //                     CardUI.clicked_card = -1;
        //                     MapManager.mapManager.clearNavTiles();
        //                     MoveButtons.nav_on = false;
        //                     player.usedcard.cardInfo.type = CardType.end;
        //                 }
        //                 break;

        //         }
        //     }
        // }
        #endregion

        if (Input.touchCount > 0)
        {
            // UI click
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //Debug.Log("ui click");
                return;
            }
            // CANNOT TOUCH ( TURN END ~ BEFORE DRAW PHASE )
            if (StageManager.stageManager.stage == StageStatus.CANNOT_TOUCH)
            {
                Debug.Log("not allowed touch");
                return;
            }

            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    First_Touch_Position = touch.position;
                    break;
                case TouchPhase.Ended:
                    Last_Touch_Position = touch.position;
                    Vector3 swipe_direction = Last_Touch_Position - First_Touch_Position;
                    if (Mathf.Abs(swipe_direction.y) > 200)
                    {
                        // 슬라이드

                        if (swipe_direction.y > 0)
                        {
                            // 위로
                            if (StageManager.stageManager.stage == StageStatus.DEFAULT)
                            {
                                // 카드 덱 ui 올리기
                                Debug.Log("swipe up");
                                cardsPanel.MoveUp();
                            }
                            else
                            {
                                Debug.Log("cant swipe up");
                                return;
                            }
                        }
                        else
                        {
                            // 아래로
                            if (StageManager.stageManager.stage == StageStatus.DECK_ON)
                            {
                                // 카드 덱 ui 내리기
                                Debug.Log("swipe down");
                                cardsPanel.MoveDown();
                            }
                            else
                            {
                                Debug.Log("cant swipe down");
                                return;
                            }
                        }
                    }
                    else
                    {
                        // 탭
                        switch (StageManager.stageManager.stage)
                        {
                            case StageStatus.BIG_CARD_ON:
                                break;
                            case StageStatus.SELECTING_MOVE:
                                {
                                    Vector3Int clickPos = GetGridPos();
                                    Player player = StageManager.stageManager.GetPlayer();
                                    int x = clickPos.x;
                                    int y = clickPos.y;
                                    clickPos.z = 0;
                                    if (x < 0 || x >= 5 + Constants.mapHeight && y < 5 && y >= 5 + Constants.mapWidth) return;
                                    clickPos -= new Vector3Int(5, 5, 0);
                                    if (MapManager.mapManager.GetTilemap(1).GetTile(clickPos) == null) return;
                                    List<Pos> path = MapManager.getPath(player.curpos.x, player.curpos.y, x - 5, y - 5, player.stat.moveType == moveType.ground);
                                    // for (int i = 0; i < path.Count; i++)
                                    // {
                                    //     Debug.Log(path[i]);
                                    // }
                                    if (path != null)
                                    {
                                        // 여기에 lineon 변수 조건달아서 경로 그리기 추가

                                        if (!line_on)
                                        {
                                            MapManager.DrawPath(path);
                                            Last_Touched_Grid = clickPos;
                                            line_on = true;
                                        }
                                        else
                                        {
                                            // 클릭한 타일이 전에 클릭한 타일과 동일 : 이동
                                            if (clickPos.x == Last_Touched_Grid.x && clickPos.y == Last_Touched_Grid.y)
                                            {
                                                // 이동
                                                Last_Touch_Position = default_Grid;
                                                MapManager.mapManager.clearNavTiles(1);
                                                MapManager.mapManager.clearNavTiles(5);
                                                MoveButtons.nav_on = false;
                                                player.remain_move -= path.Count - 1;
                                                player.move(path);
                                                MapManager.mapManager.map[player.curpos.x, player.curpos.y] &= 0x0000FFFF;
                                                player.curpos.x = x - 5;
                                                player.curpos.y = y - 5;
                                                MapManager.mapManager.map[x - 5, y - 5] |= 0x00010000;
                                            }
                                            // 아니면 : 클리어 후 새로 선 그리기
                                            else
                                            {
                                                Last_Touch_Position = clickPos;
                                                MapManager.mapManager.clearNavTiles(1);
                                                MapManager.mapManager.clearNavTiles(5);
                                                MapManager.DrawPath(path);
                                            }
                                        }
                                    }
                                    break;

                                }
                            case StageStatus.SELECTING_CARD:
                                {
                                    Vector3Int clickPos = GetGridPos();
                                    break;

                                }
                        }
                    }
                    First_Touch_Position = Vector3.zero;
                    Last_Touch_Position = Vector3.zero;
                    break;
                case TouchPhase.Moved:
                    break;
            }
        }
    }
    Vector3Int GetGridPos()
    {
        Vector3 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
        return MapManager.mapManager.GetTilemap(0).WorldToCell(mouseWorldPos);
    }

}
