using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potionMaking : MonoBehaviour
{
    int iCount = 0;
    int waterCount = 0;
    bool fruit = true;
    public int ingredientNum;
    public GameObject sleepPotion;
    bool potionSpawn = true;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ingredient")) //if object is an ingredient, add to ingredient count
        {
            Destroy(collision.gameObject);
            iCount++;
        }
        if(collision.gameObject.CompareTag("Food") && fruit) //if object is food, add to count and don't let any more food count
        {
            Destroy(collision.gameObject);
            fruit = false;
            iCount++;
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Water"))
        {
            waterCount++;
        }
    }
    private void Update()
    {
        if (iCount == ingredientNum && waterCount > 200 && potionSpawn) //if ingredient count is high enough, you win
        {
            potionSpawn = false;
            Debug.Log("Sleeping Potion");
            Instantiate(sleepPotion, new Vector3(transform.localPosition.x,
                                                 transform.localPosition.y + 10,
                                                 transform.localPosition.z), transform.rotation);
        }
    }
}
