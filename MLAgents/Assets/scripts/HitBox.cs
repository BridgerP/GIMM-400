using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitBox : MonoBehaviour
{
    public Color defaultColour = Color.green;
    public Color hitColour = Color.red;
    public Collider coll;
    public bool wasHit { get; private set; }
    // private Material material;

    public void HasBeenHit()
    {
        wasHit = true;
        // material.SetColor("_Color", hitColour);
        gameObject.SetActive(false);
    }

    public void ResetSelf()
    {
        wasHit = false;
        gameObject.SetActive(true);
        // material.SetColor("_Color", defaultColour);
    }

    public void ToggleMesh(bool toggle)
    {
        // gameObject.GetComponent<MeshRenderer>().enabled = toggle;
    }

    private void Awake()
    {
        // MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        // material = meshRenderer.material;
        coll = gameObject.GetComponent<Collider>();
    }
}

