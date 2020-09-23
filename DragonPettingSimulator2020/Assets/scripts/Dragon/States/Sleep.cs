using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : dragState
{
    public Sleep(Dragon dragon) : base(dragon) { }
    public override void OnEnter()
    {
        Debug.Log("Asleep!");
        Time.timeScale = 0.5f;
        dragon.agent.SetDestination(dragon.transform.position);
    }
    public override void OnUpdate()
    {
        
    }
    public override void OnExit()
    {

    }
}
