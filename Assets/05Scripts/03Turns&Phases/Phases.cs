using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Phases : ScriptableObject
{
    public string phaseName;
    public bool forceExit;
    public abstract bool IsComplete();

    [System.NonSerialized]
    protected bool PhaseInit;

    public virtual void OnStartPhase()
    {
        if (PhaseInit)
            return;
        PhaseInit = true;
    }

    public virtual void OnEndPhase()
    {
        PhaseInit = false;
        forceExit = false;
    }



}
