using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : dragState 
{
    int count;
    float boredCount;
    public Wander(Dragon dragon) : base(dragon) { }
    public override void OnEnter()
    {
        //Debug.Log("Wandering");
        count = 0;
        RandomTarget();
    }
    public override void OnUpdate()
    {
        count++;
        boredCount++;
        //Debug.Log(count);
        if(count > 1000)
        {
            RandomTarget();
            count = 0;
        }
        if(boredCount == 3500)
        {
            dragon.changeState(new Bored(dragon));
        }
        
        if (Vector3.Distance(dragon.player.transform.position, dragon.transform.position) < dragon.playerViewRange)
        {
            dragon.changeState(new FollowPlayer(dragon));
        }
    }
    public override void OnExit()
    {
        //Debug.Log("Wander End");
    }
    private void RandomTarget()
    {
        Vector3 newTarget = new Vector3(dragon.transform.position.x + Random.Range(-25, 25),
                                        dragon.transform.position.y,
                                        dragon.transform.position.z + Random.Range(-25, 25));

        dragon.agent.SetDestination(newTarget);
    }
}
