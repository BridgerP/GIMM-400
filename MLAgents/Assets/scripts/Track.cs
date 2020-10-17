using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{

    public List<HitBox> hitBoxes;
    public bool isTraining = false;
    private Dictionary<Collider, HitBox> hitBoxDictionary;

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

    public HitBox GetHitBoxFromCollider(Collider collider)
    {
        return hitBoxDictionary[collider];
    }

    private void Awake()
    {
        hitBoxDictionary = new Dictionary<Collider, HitBox>();
        hitBoxes = new List<HitBox>();
    }
}
