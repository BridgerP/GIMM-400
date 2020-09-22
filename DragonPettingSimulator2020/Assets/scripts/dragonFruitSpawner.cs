using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragonFruitSpawner : MonoBehaviour
{
    public GameObject dragonFruit;
    public Transform spawner;

    int count;
    void Start()
    {
        Instantiate(dragonFruit, spawner.position, spawner.rotation); //at the start of the game, instantiate a dragonfruit
    }

    // Update is called once per frame
    void Update()
    {
        count++;
        if(count == 5000)
        {
            Instantiate(dragonFruit, spawner.position, spawner.rotation); //spawn a new fruit every 5000 ticks
            count = 0;
        }
    }
}
