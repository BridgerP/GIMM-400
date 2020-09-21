using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FollowFood : dragState
{
    int count;
    bool toy;
    public FollowFood(Dragon dragon) : base(dragon) { }
    public override void OnEnter()
    {
        dragon.agent.stoppingDistance = 1.5f;
        if(dragon.toyTime)
        {
            Debug.Log("drop toy");
            dragon.toy.GetComponent<grabbable>().unGrab(8, Vector3.back);
            toy = true;
        }
    }
    public override void OnUpdate()
    {
        dragon.agent.SetDestination(dragon.food.transform.position);
        if(Vector3.Distance(dragon.food.transform.position, dragon.transform.position) < 2)
        {
            count++;
            if(count > 600)
            {
                GameObject.Destroy(dragon.food);
                dragon.food = null;
                if (dragon.toyTime) dragon.changeState(new FollowToy(dragon));
                else dragon.changeState(new Wander(dragon));
            }
        }
    }
    public override void OnExit()
    {

    }
}
