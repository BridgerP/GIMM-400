using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FollowFood : dragState
{
    int count;
    bool foodGrabbed;
    public FollowFood(Dragon dragon) : base(dragon) { }
    public override void OnEnter()
    {
        foodGrabbed = false;
        count = 0;
        dragon.agent.stoppingDistance = .3f;
        if(dragon.toyTime)
        {
            Debug.Log("drop toy");
            dragon.toy.GetComponent<grabbable>().unGrab(8, Vector3.back);
        }
    }
    public override void OnUpdate()
    {
        if (dragon.food)
        {
            dragon.agent.SetDestination(dragon.food.transform.position);
            if (Vector3.Distance(dragon.food.transform.position, dragon.transform.position) < 2)
            {
                count++;
                //Debug.Log(count);
                if (count > 600)
                {
                    GameObject.Destroy(dragon.food);
                    dragon.food = null;
                    if (dragon.toyTime) dragon.changeState(new FollowToy(dragon));
                    else dragon.changeState(new Wander(dragon));
                }
            }
        }
        else if(count < 599) dragon.changeState(new Mad(dragon));
    }
    public override void OnExit()
    {

    }
}
