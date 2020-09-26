using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class flammable : MonoBehaviour
{
    public ParticleSystem ps;
    bool flaming;
    int count;
    private void Start()
    {
        ps.Stop(); //stop particle system up front
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Fire")) //if hit with fire, ignite
        {
            ps.Play();
            flaming = true;
        }
        if (other.CompareTag("Water")) //if hit with water, put out fire
        {
            ps.Stop();
            flaming = false;
            count = 0;
        }
    }
    private void Update()
    {
        if (flaming) count++;
        if (count > 1500) Destroy(gameObject); //if flaming long enough, object destroyed
    }
}
