using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFlammable : MonoBehaviour
{
    int health;
    ParticleSystem ps;
    bool flaming;
    private void Start()
    {
        flaming = false;
        ps = GetComponent<ParticleSystem>();
        ps.Stop();
        health = 5000;
    }
    private void Update()
    {
        if (flaming)
        {
            health--;
            Debug.Log(health);
        }
        if(health == 0)
        {
            Time.timeScale = 0;
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Fire"))
        {
            ps.Play();
            flaming = true;
        }
        if (other.CompareTag("Water"))
        {
            ps.Stop();
            flaming = false;
        }
    }
}
