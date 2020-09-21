﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPlayer : dragState
{
    public ReturnToPlayer(Dragon dragon) : base(dragon) { }
    public override void OnEnter()
    {
        //Debug.Log("Returning to Player");
    }
    public override void OnUpdate()
    {
        dragon.agent.SetDestination(dragon.player.transform.position);
        if(Vector3.Distance(dragon.transform.position, dragon.player.transform.position) < 10)
        {
            dragon.toy.GetComponent<grabbable>().unGrab(5, Vector3.forward);
            dragon.toyTime = false;
            dragon.toy = null;
            dragon.changeState(new FollowPlayer(dragon));
        }
    }
    public override void OnExit()
    {
        
        //Debug.Log("Dropped Toy");
    }
}
