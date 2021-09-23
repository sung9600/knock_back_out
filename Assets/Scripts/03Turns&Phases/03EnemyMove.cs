using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : Phases
{
    public override void OnStartPhase()
    {
        TurnManager.turnManager.indicator.SetText("enemy move phase");
    }
    public override bool IsComplete()
    {
        return true;
    }
}
