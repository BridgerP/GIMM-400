using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bored : dragState
{
    int count;
    int madCount;
    public Bored(Dragon dragon) : base(dragon) { }
    public override void OnEnter()
    {
        Debug.Log("bored");
        dragon.fire.Play();
    }
    public override void OnUpdate()
    {
        count++;
        madCount++;
        if (dragon.toyTime)
        {
            dragon.changeState(new FollowToy(dragon));
        }
        if (count > 120)
        {
            RandomTarget();
            count = 0;
        }
        if (madCount > 420)
        {
            dragon.changeState(new Mad(dragon));
        }
    }
    public override void OnExit()
    {
        dragon.fire.Stop();
    }
    private void RandomTarget()
    {
        Vector3 newTarget = new Vector3(dragon.transform.position.x + Random.Range(-25, 25),
                                        dragon.transform.position.y,
                                        dragon.transform.position.z + Random.Range(-25, 25));

        dragon.agent.SetDestination(newTarget);
    }
}
