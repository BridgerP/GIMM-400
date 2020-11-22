using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{

    public List<HitBox> hitBoxes;
    public bool isTraining = false;

    public void ResetTrack()
    {
        foreach (HitBox hitBox in hitBoxes)
        {
            hitBox.ResetSelf();
            if (isTraining)
            {
                hitBox.ToggleMesh(true);
            }
            else
            {
                hitBox.ToggleMesh(false);
            }
        }
    }
}
