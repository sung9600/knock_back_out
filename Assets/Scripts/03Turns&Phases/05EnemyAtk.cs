using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtk : Phases
{
    public override void OnStartPhase()
    {
        TurnManager.turnManager.indicator.SetText("enemy atk phase");
    }
    public override bool IsComplete()
    {
        return true;
    }
}