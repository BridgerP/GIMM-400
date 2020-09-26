using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Dragon : MonoBehaviour
{
    public dragState currentState;
    public GameObject player;
    public NavMeshAgent agent;

    public float playerViewRange;
    public GameObject food;
    public GameObject toy;
    public bool toyTime;

    public ParticleSystem fire;
    public UIManager uI;
    public playerFlammable playerFire;
    private void Start()
    {
        playerFire = player.GetComponentInChildren<playerFlammable>();
        fire.Stop();
        toyTime = false;
        currentState = new Wander(this); //stops fire and the toy state, begins wandering
        //Debug.Log(currentState);
    }
    private void FixedUpdate()
    {
        //Debug.Log(toyTime);
        currentState.OnUpdate(); //run update behavior based on current state
    }
    public void changeState(dragState newState)
    {
        currentState.OnExit(); //run exit function of state

        currentState = newState; //change state

        currentState.OnEnter(); //run start function of state
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("SleepPotion"))
        {
            Destroy(collision.gameObject);
            changeState(new Sleep(this));
        }
    }
}
