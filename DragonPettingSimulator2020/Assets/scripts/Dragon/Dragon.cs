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
    private void Start()
    {
        fire.Stop();
        toyTime = false;
        currentState = new Wander(this);
        //Debug.Log(currentState);
    }
    private void Update()
    {
        //Debug.Log(toyTime);
        currentState.OnUpdate();
    }
    public void changeState(dragState newState)
    {
        currentState.OnExit();

        currentState = newState;

        currentState.OnEnter();
    }
}
