using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Characters
{
    protected string _name;
    protected int enemy_number;

    [SerializeField]
    protected Characters atk_target;
    [SerializeField]
    protected Pos atk_dir = null;
    protected Pos warning_pos = null;
    public bool turn_done = false;
    [SerializeField]
    protected List<Characters> targets = new List<Characters>();

    protected enum type
    {
        warrior,
        archer,
        artillery,
        buffer,
        architecture
    }
    private void Start()
    {
        StageManager.stageManager.GetCharactersList().Add(this);
    }


    public abstract void SetTarget();
    public abstract void Warning();
    public abstract void Skill();

}
