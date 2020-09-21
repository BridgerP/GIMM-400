using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class dragState
{
    protected Dragon dragon;
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
    public dragState(Dragon dragon)
    {
        this.dragon = dragon;
    }
}
