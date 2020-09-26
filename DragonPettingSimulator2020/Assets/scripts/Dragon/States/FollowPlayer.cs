using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FollowPlayer : dragState
{
    int count;
    public FollowPlayer(Dragon dragon) : base(dragon) { }
    public override void OnEnter()
    {
        //Debug.Log("following player");
        dragon.agent.SetDestination(dragon.player.transform.position);
        dragon.agent.stoppingDistance = 4; //change stopping distance to not get too close to player
    }
    public override void OnUpdate()
    {
        dragon.agent.SetDestination(dragon.player.transform.position); //follow player
        count++;
        if (Vector3.Distance(dragon.player.transform.position, dragon.transform.position) > dragon.playerViewRange * 1.1f)
        {
            dragon.changeState(new Wander(dragon)); //if player is too far, wander
        }
        if (Vector3.Distance(dragon.player.transform.position, dragon.transform.position) < dragon.playerViewRange && dragon.toyTime)
        {
            dragon.changeState(new FollowToy(dragon)); //if player throws a toy, chase the toy
        }
        if(count > 450)
        {
            dragon.changeState(new Mad(dragon)); //if the player ignores the dragon, gets mad
        }
    }
    public override void OnExit()
    {
        //Debug.Log(dragon.toyTime);
        dragon.agent.stoppingDistance = 0.5f;
    }
}
