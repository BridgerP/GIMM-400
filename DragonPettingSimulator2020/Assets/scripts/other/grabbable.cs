using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class grabbable : MonoBehaviour
{
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();//find object rigidbody
    }
    public void grab(GameObject parent)
    {
        transform.SetParent(parent.transform);//when grabbed, parent to player
        rb.isKinematic = true;//and can't interact with other objects
        if (parent.GetComponent<Dragon>())
        {
            transform.localPosition = new Vector3(-0.065f, .001f, 0f);
        }
    }
    public void unGrab(float force, Vector3 dir)
    {   
        rb.isKinematic = false;
        if (gameObject.CompareTag("Ingredient")) force *= 0.3f;
        rb.velocity = dir * force; //when thrown, sets direction and force from player
        transform.SetParent(null);
    }
}
