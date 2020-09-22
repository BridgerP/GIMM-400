using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDetector : MonoBehaviour
{
    public Dragon dragon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Food"))//if object in radius is food, change state
        {
            Debug.Log("Food Seen");
            dragon.food = other.gameObject;
            dragon.changeState(new FollowFood(dragon));
        }
    }
}
