using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mad : dragState
{
    public Mad(Dragon dragon) : base(dragon) { }
    public override void OnEnter()
    {
        Debug.Log("MAD");
        dragon.fire.Play();
    }
    public override void OnUpdate()
    {
        dragon.agent.SetDestination(dragon.player.transform.position);
    }
    public override void OnExit()
    {
        dragon.fire.Stop();
    }
}
