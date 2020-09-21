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
        dragon.agent.stoppingDistance = 8;
    }
    public override void OnUpdate()
    {
        dragon.agent.SetDestination(dragon.player.transform.position);
        count++;
        if (Vector3.Distance(dragon.player.transform.position, dragon.transform.position) > dragon.playerViewRange * 1.5f)
        {
            dragon.changeState(new Wander(dragon));
        }
        if (Vector3.Distance(dragon.player.transform.position, dragon.transform.position) < dragon.playerViewRange && dragon.toyTime)
        {
            dragon.changeState(new FollowToy(dragon));
        }
        if(count > 4000)
        {
            dragon.changeState(new Mad(dragon));
        }
    }
    public override void OnExit()
    {
        //Debug.Log(dragon.toyTime);
        dragon.agent.stoppingDistance = 0.5f;
    }
}
