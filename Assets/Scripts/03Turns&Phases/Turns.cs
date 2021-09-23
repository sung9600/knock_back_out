using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turns : ScriptableObject
{
    public int index;
    public Phases[] phases;

    protected bool TurnInit;
    protected virtual bool InitTurn()
    {
        return false;
    }
    public bool Execute()
    {
        if (!TurnInit)
            InitTurn();
        if (index > phases.Length - 1) return true;
        bool result = false;
        phases[index].OnStartPhase();
        if (phases[index].IsComplete())
        {
            //Debug.Log(string.Format("phase {0} done", index));
            phases[index].OnEndPhase();
            index++;
            TurnManager.turnManager.phase++;
            if (index > phases.Length - 1)
            {
                //한턴 종료
                //index = 0;
                result = true;
                return result;
            }
        }
        return result;
    }


    public void EndCurrentPhase()
    {
        phases[index].forceExit = true;
    }
}
