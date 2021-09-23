using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapGimmik : Phases
{
    public override void OnStartPhase()
    {
        TurnManager.turnManager.indicator.SetText("map phase");
    }
    public override bool IsComplete()
    {
        return true;
    }
}
