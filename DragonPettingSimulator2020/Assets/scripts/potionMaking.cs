using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potionMaking : MonoBehaviour
{
    int iCount = 0;
    int waterCount = 0;
    bool fruit = false;
    public int ingredientNum;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ingredient")) //if object is an ingredient, add to ingredient count
        {
            Destroy(collision.gameObject);
            iCount++;
        }
        if(collision.gameObject.CompareTag("Food") && !fruit) //if object is food, add to count and don't let any more food count
        {
            Destroy(collision.gameObject);
            fruit = true;
            iCount++;
        }
        if(iCount == ingredientNum && waterCount > 200) //if ingredient count is high enough, you win
        {
            Time.timeScale = 0f;
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Water"))
        {
            waterCount++;
        }
    }
}
