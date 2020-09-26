using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFlammable : MonoBehaviour
{
    public int health;
    public Transform healthBar;
    ParticleSystem ps;
    bool flaming;

    public UIManager uI;
    private void Start()
    {
        flaming = false;
        ps = GetComponent<ParticleSystem>();
        ps.Stop();
        health = 500;
    }
    private void FixedUpdate()
    {
        if (flaming)
        {
            health--;
            Debug.Log(health);
        }
        if(health == 0)
        {
            uI.gameEnd();
        }
        healthBar.localScale = new Vector3((float)health / 500, 1, 1);
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
    public void PutOutFire()
    {
        ps.Stop();
        flaming = false;
    }
}
