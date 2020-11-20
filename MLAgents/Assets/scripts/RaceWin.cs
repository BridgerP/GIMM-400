using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceWin : MonoBehaviour
{
    public GameObject WinScreen;

    // Update is called once per frame
    private void OnTriggerEnter()
    {
        WinScreen.SetActive(true);
    }
}
