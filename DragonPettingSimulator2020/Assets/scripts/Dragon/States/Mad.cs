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
        dragon.agent.stoppingDistance = 1f;
        dragon.fire.Play(); //begin shooting fire
    }
    public override void OnUpdate()
    {
        dragon.agent.SetDestination(dragon.player.transform.position); //follows the player
    }
    public override void OnExit()
    {
        dragon.fire.Stop(); //stop shooting fire
    }
}
