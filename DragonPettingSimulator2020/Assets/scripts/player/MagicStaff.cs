using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MagicStaff : MonoBehaviour
{
    public float range;
    public LayerMask layerMask;
    private GameObject grabbableObject;

    public float throwForce;

    bool grabbing;
    int counter;
    bool count;
    bool magic;

    public ParticleSystem ps;

    public Dragon dragon;
    private void Start()
    {
        ps.Stop(); //Stop water particle system up front
        magic = true;
    }
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, range, layerMask))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);//creates a raycast to detect if objects are in the crosshair and close enough to grab

            if (hit.collider.gameObject.GetComponent<grabbable>())
            {
                grabbableObject = hit.collider.gameObject;//if object is grabbable, set to grabbableObject
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * range, Color.white);

            if(grabbableObject != null)
            {
                grabbableObject = null; //otherwise nullify grabbableObject
            }
        }
        if (magic)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (grabbableObject) //if you have a potential object to grab
                {
                    if (grabbing) //but youre already grabbing, you will begin the process to throw the object
                    {
                        count = true;
                        grabbing = false;
                    }
                    else
                    {
                        grabSpellStart(); //if not, grab object
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (!grabbing && grabbableObject)//!grabbing because it is set to false when clicked
                {
                    grabSpellEnd();
                }
            }
            if (Input.GetMouseButtonDown(1) && !grabbing)
            {
                waterSpellStart();
            }
            if (count)
            {
                counter++;
            }
        }
    }
    void waterSpellStart()
    {
        ps.Play();
        magic = false;
        StartCoroutine(spellCountdown());
    }
    void waterSpellEnd()
    {
        ps.Stop();
        StartCoroutine(spellCooldown(3));
    }
    void grabSpellStart()
    {
        grabbableObject.GetComponent<grabbable>().grab(gameObject);
        grabbing = true;
        StartCoroutine(spellCountdown());
    }
    private IEnumerator spellCountdown()
    {
        yield return new WaitForSeconds(3);
        if(grabbableObject)
            grabSpellEnd();
        if (ps.isPlaying)
            waterSpellEnd();
    }
    private IEnumerator spellCooldown(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        magic = true;
    }
    void grabSpellEnd()
    {
        magic = false;
        count = false;
        float force = counter * throwForce;
        //set throwing force based on how long the button is held
        //Debug.Log(force);
        counter = 0;
        if (force > 17) force = 17;
        //limit the force max
        grabbableObject.GetComponent<grabbable>().unGrab(force, transform.forward);
        if (grabbableObject.CompareTag("Toy") && Vector3.Distance(dragon.transform.position, transform.position) < 20)
        {
            dragon.toyTime = true;
            dragon.toy = grabbableObject;
        }
        grabbableObject = null;
        StartCoroutine(spellCooldown(1));
    }
}
