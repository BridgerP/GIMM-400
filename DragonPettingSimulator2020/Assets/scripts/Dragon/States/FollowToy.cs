using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowToy : dragState
{
    public FollowToy(Dragon dragon) : base(dragon) { }
    int count;
    public override void OnEnter()
    {
        //Debug.Log("Chasing Toy");
        count = 0;
        dragon.toyTime = true;
        dragon.agent.stoppingDistance = .25f; //changes stopping distance to get closer to toy
    }
    public override void OnUpdate()
    {
        //Debug.Log(dragon.toyTime);
        count++;
        dragon.agent.SetDestination(dragon.toy.transform.position);
        if(Vector3.Distance(dragon.toy.transform.position, dragon.gameObject.transform.position) < .5) //when the dragon is close enough, grab toy
        {
            dragon.toy.GetComponent<grabbable>().grab(dragon.gameObject);
            dragon.changeState(new ReturnToPlayer(dragon));
        }
        if (count > 450) dragon.changeState(new Mad(dragon));
    }
    public override void OnExit()
    {
        //Debug.Log("Toy Grabbed");
    }
}
