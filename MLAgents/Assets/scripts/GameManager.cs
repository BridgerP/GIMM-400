using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    void Start()
    {
        if(Instance != null)
        {
            Instance = this;
        }
    }

    public void CheckForWin(int lap, bool isPlayer)
    {
        if(isPlayer && lap > 3)
        {
            Debug.Log("Player Won");
        }
        else if(!isPlayer && lap > 3)
        {
            Debug.Log("Agent Won");
        }
    }
}
