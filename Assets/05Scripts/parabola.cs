using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class parabola : MonoBehaviour
{
    public void shoot(Vector3Int target)
    {
        Vector3 target_ = MapManager.mapManager.GetTilemap(0).GetCellCenterWorld(target);
        Vector3 target_rotation;
        if (transform.position.x > target_.x)
        {
            //left : z rotation 0->180
            target_rotation = new Vector3(0, 0, 180);
        }
        else
        {
            //right : z rotation 0->-180
            target_rotation = new Vector3(0, 0, -180);
        }
        transform.DOJump(target_ + new Vector3(0, 0.5f, 0), 1.5f, 1, 1f, false)
        .SetEase(Ease.InQuad)
        .OnPlay(() =>
        {
            transform.DORotate(target_rotation, 1f).SetEase(Ease.InQuad)
            .OnComplete(() => Destroy(gameObject));
        })
        .OnComplete(() =>
        {
            //MapManager.mapManager.update_tileanims((target.x), (target.y), 1);
            if (!MapManager.isEmptyTile(target.x, target.y))
            {
                Characters hit_target = StageManager.stageManager.GetCharacterByVector3Int(target);
                if (hit_target != null)
                {
                    Debug.Log(hit_target.name);
                    hit_target.GetHit();
                }
            }
            transform.parent.GetComponent<SpriteRenderer>().flipX = false;
            transform.parent.GetComponent<Characters>().status = Character_status.waiting;
        }
        );
    }
}
