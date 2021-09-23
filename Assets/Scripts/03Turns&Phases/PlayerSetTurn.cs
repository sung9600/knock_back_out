using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetTurn : Turns
{
    private class PlayerSetPhases : Phases
    {
        public override bool IsComplete()
        {
            return false;
        }
    }
    private bool turnover;
    protected override bool InitTurn()
    {
        turnover = false;
        phases = new Phases[5];
        phases[0] = ScriptableObject.CreateInstance<mapGimmik>();
        phases[1] = ScriptableObject.CreateInstance<DrawPhase>();
        phases[2] = ScriptableObject.CreateInstance<EnemyMove>();
        phases[3] = ScriptableObject.CreateInstance<PlayerPhase>();
        phases[4] = ScriptableObject.CreateInstance<EnemyAtk>();

        return turnover;
    }



}

