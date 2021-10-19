using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
            // ui click 예외처리
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //Debug.Log("input1");
                return;
            }
            if (TurnManager.turnManager.phase != phase.player_turn)
            {
                //Debug.Log("input2");
                return;
            }
            // 처음 캐릭터 배치 예외처리
            Vector3Int gridMousePos = GetGridPos();
            int x = gridMousePos.x;
            int y = gridMousePos.y;
            gridMousePos.z = 0;
            Player player = StageManager.stageManager.GetPlayer();
            if (!Cards.usingcard)
            {
                // 이동클릭
                // 5x5그리드 안 클릭인경우
                if (x >= 5 && x < 5 + Constants.mapHeight && y >= 5 && y < 5 + Constants.mapWidth)
                {
                    // 이동버튼 눌렀고, 이동중 아닌경우
                    if (MoveButtons.nav_on && player.status == Character_status.waiting)
                    {
                        // 플레이어 이동범위 표시한거 지우고 클릭이 유효하면 해당위치로 이동
                        // tilemap 범위가 맵 밖인경우
                        if (MapManager.mapManager.GetTilemap(2).GetTile(gridMousePos - new Vector3Int(5, 5, 0)) != null)
                        {
                            List<Pos> path = MapManager.getPath(player.curpos.x, player.curpos.y, x - 5, y - 5, player.stat.moveType == moveType.ground);
                            if (path != null)
                            {
                                player.remain_move -= path.Count - 1;
                                player.move(path);
                                MapManager.mapManager.map[player.curpos.x, player.curpos.y] &= 0x0000FFFF;

                                player.curpos.x = x - 5;
                                player.curpos.y = y - 5;
                                MapManager.mapManager.map[x - 5, y - 5] |= 0x00010000;
                            }
                        }
                    }
                    MapManager.mapManager.clearNavTiles();
                    MoveButtons.nav_on = false;
                }

            }
            else if (player.usedcard.cardInfo.type != CardType.end)
            {
                switch (player.usedcard.cardInfo.type)
                {
                    case CardType.Attack_direct:
                        // 직사공격
                        if (MapManager.mapManager.GetTilemap(2).GetTile(gridMousePos - new Vector3Int(5, 5, 0)) != null)
                        {
                            Pos direction = new Pos(x - 5, y - 5) - player.curpos;
                            bool up = direction.x + direction.y > 0;
                            bool left = direction.x < 0 || (direction.x == 0 && direction.y > 0);
                            if (!left) player.gameObject.GetComponent<SpriteRenderer>().flipX = true;
                            for (int i = 1; i < Constants.mapHeight; i++)
                            {
                                int nx, ny;
                                if (left)
                                {
                                    if (up)
                                    {
                                        nx = player.curpos.x;
                                        ny = player.curpos.y + i;
                                    }
                                    else
                                    {
                                        nx = player.curpos.x - i;
                                        ny = player.curpos.y;
                                    }
                                }
                                else
                                {
                                    if (up)
                                    {
                                        nx = player.curpos.x + i;
                                        ny = player.curpos.y;
                                    }
                                    else
                                    {
                                        nx = player.curpos.x;
                                        ny = player.curpos.y - i;
                                    }
                                }
                                // todo : nx,ny에 적있는지 체크
                                if (!MapManager.checkWidthHeight(nx, ny))
                                {
                                    // todo : 끝에 도달해서 총알 튀는 애니메이션
                                    break;
                                }
                                if (MapManager.mapManager.map[nx, ny] == (int)tileType.rock)
                                {
                                    break;
                                }
                                MapManager.mapManager.update_tileanims(nx, ny, 1);
                            }
                            Cards.usingcard = false;
                            Cards.clicked_card = -1;
                            MapManager.mapManager.clearNavTiles();
                            MoveButtons.nav_on = false;
                            player.usedcard.cardInfo.type = CardType.end;
                        }
                        break;
                    case CardType.Attack_indirect:
                        // 곡사공격
                        if (MapManager.mapManager.GetTilemap(2).GetTile(gridMousePos - new Vector3Int(5, 5, 0)) != null)
                        {
                            Pos direction = new Pos(x - 5, y - 5) - player.curpos;
                            bool left = direction.x < 0 || (direction.x == 0 && direction.y > 0);
                            if (!left) player.gameObject.GetComponent<SpriteRenderer>().flipX = true;
                            player.status = Character_status.attacking;
                            // Todo : animation
                            player.GetAnimator().SetTrigger("Shoot");
                            GameObject bullet = GameObject.Instantiate(player.bullet_prefab, player.transform.position, Quaternion.identity, player.transform);
                            bullet.AddComponent<parabola>();
                            bullet.GetComponent<parabola>().shoot(new Vector3Int(gridMousePos.x - 5, gridMousePos.y - 5, 0));
                            //StageManager.stageManager.mapManager.update_tileanims((x - 5) * 5 + (y - 5), 1);
                            Cards.usingcard = false;
                            Cards.clicked_card = -1;
                            MapManager.mapManager.clearNavTiles();
                            MoveButtons.nav_on = false;
                            player.usedcard.cardInfo.type = CardType.end;
                        }
                        break;
                    case CardType.Attack_near:
                        // 근접공격
                        if (MapManager.mapManager.GetTilemap(2).GetTile(gridMousePos - new Vector3Int(5, 5, 0)) != null)
                        {
                            Pos direction = new Pos(x - 5, y - 5) - player.curpos;
                            bool left = direction.x < 0 || (direction.x == 0 && direction.y > 0);
                            if (!left) player.gameObject.GetComponent<SpriteRenderer>().flipX = true;
                            player.status = Character_status.attacking;
                            player.GetAnimator().SetTrigger("Attack");
                            MapManager.mapManager.update_tileanims((x - 5), (y - 5), 1);
                            Cards.usingcard = false;
                            Cards.clicked_card = -1;
                            MapManager.mapManager.clearNavTiles();
                            MoveButtons.nav_on = false;
                            player.usedcard.cardInfo.type = CardType.end;
                        }
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
}
