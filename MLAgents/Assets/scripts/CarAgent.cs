using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Scripting.APIUpdating;

public class CarAgent : Agent
{
    // Public vars
    public Vector3 START_POS = new Vector3(352.9f, 22.37f, 189.74f);
    public float accelerateForce = 2f;
    public float brakingForce = 0.5f;
    public float speed = 15f; // 5f;
    public float turnSpeed = 150f;
    public bool trainingMode = true;
    public float reward = .1f; // .02f;
    public float offTrackValue = -.5f;
    public int currentTrackPos { get; private set; }
    public int lap = 1;

    // Private vars
    private Track track;
    private HitBox destination;
    private float curDist;
    private float prevDist;
    new private Rigidbody rigidbody;
    private float smoothTurnChange = 0f;
    private bool frozen = false;
    public override void Initialize()
    {
        rigidbody = GetComponent<Rigidbody>();
        track = GetComponentInParent<Track>();

        // If not in training mode, no max step, play forever
        if (!trainingMode)
        {
            MaxStep = 0;
        }
    }

    public override void OnEpisodeBegin()
    {
        if (trainingMode)
        {
            track.ResetTrack();
        }

        currentTrackPos = 0;

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        MoveToStartingPosition();

        destination = null;
        UpdateDestination();
    }


    /// <summary>
    /// Called when action is received from either the player input or the nerual network
    /// 
    /// vectorAction[i] represents:
    /// Index 0: acceleration force (range: braking = -1 to accelerating = 1);
    /// Index 1: turning force (range: left = -1 to right = 1)
    /// </summary>
    /// <param name="vectorAction"></param>
    public override void OnActionReceived(float[] vectorAction)
    {
        if (frozen){return;}

        Vector3 moveForce = Vector3.zero;
        if (vectorAction[0] > 0)
        {
            moveForce = (vectorAction[0] * accelerateForce) * transform.forward;
        }
        else if (vectorAction[0] < 0)
        {
            moveForce = (vectorAction[0] * brakingForce) * transform.forward;
        }
        rigidbody.AddForce(moveForce);

        Vector3 rotationVector = transform.rotation.eulerAngles;
        float rotationChange = vectorAction[1];
        smoothTurnChange = Mathf.MoveTowards(smoothTurnChange, rotationChange, 2f * Time.fixedDeltaTime);

        float newRotation = rotationVector.y + smoothTurnChange * Time.fixedDeltaTime * turnSpeed;
        transform.rotation = Quaternion.Euler(0f, newRotation, 0f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // If no destination, return an empty array
        if (destination == null)
        {
            sensor.AddObservation(new float[8]);
            return;
        }

        //local rotation (4 observations)
        sensor.AddObservation(transform.localRotation.normalized);

        // Get a vector from the vechile to the next checkpoint
        Vector3 toCheckpoint = destination.transform.position - transform.position;

        // Observe a normalized vector pointing to the next checkpoint (3 observations)
        sensor.AddObservation(toCheckpoint.normalized);

        // Observe a dot product that indicates whether the vehicle is pointing toward the checkpoint (1 observation)
        // +1 means that the vehicle is pointing directly at the checkpoint, -1 means directly away
        sensor.AddObservation(Vector3.Dot(transform.forward.normalized, -destination.transform.forward.normalized));

        // 8 total observations
    }

    public void FreezeAgent()
    {
        Debug.Assert(trainingMode == false, "Freeze/Unfreeze not supported in training");
        frozen = true;
        rigidbody.Sleep();
    }

    public void UnfreezeAgent()
    {
        Debug.Assert(trainingMode == false, "Freeze/Unfreeze not supported in training");
        frozen = false;
        rigidbody.WakeUp();
    }

    private void MoveToStartingPosition()
    {
        transform.position = START_POS;
    }

    private void UpdateDestination()
    {
        if (destination == null)
        {
            destination = track.hitBoxes[0];
        }
        else if (track.hitBoxes.IndexOf(destination) >= track.hitBoxes.Count - 1)
        {
            track.ResetTrack();
            destination = track.hitBoxes[0];
            lap += 1;
            if (!trainingMode)
            {
                GameManager.Instance.CheckForWin(lap, false);
            }
        }
        else
        {
            destination = track.hitBoxes[track.hitBoxes.IndexOf(destination) + 1];
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("HitBox"))
        {
            HitBox hitBox = track.GetHitBoxFromCollider(collider);
            if (hitBox.gameObject.name.Equals(destination.gameObject.name))
            {
                hitBox.HasBeenHit();
                currentTrackPos += 1;
                UpdateDestination();
                if (trainingMode)
                {
                    float bonus1 = (reward / 2.0f) * (rigidbody.velocity.magnitude / speed);
                    float bonus2 = (reward / 2.0f) * Vector3.Dot((hitBox.gameObject.transform.position - transform.position), transform.forward);
                    AddReward(1 + bonus1 + bonus2);
                }
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // TODO: THIS MIGHT HAVE TO BE ADDING NEGATIVE FOR LEAVING TRACK INSTEAD OF HITTING BOUNDARY
        if (trainingMode && collision.collider.CompareTag("boundary"))
        {
            AddReward(-0.25f);
        }
    }

    private void Update()
    {
        if (destination != null)
        {
            Debug.DrawLine(transform.position, destination.gameObject.transform.position, Color.magenta);
        }
    }

    private void FixedUpdate()
    {
        if (trainingMode && destination != null)
        {
            float bonus1 = (reward / 2.0f) * (rigidbody.velocity.magnitude / speed);
            AddReward(bonus1);

            Transform tOfNextCheckPoint = destination.GetComponent<Transform>();
            float distanceToAgent = Vector3.Distance(transform.position, tOfNextCheckPoint.position);

            curDist = distanceToAgent;

            if (prevDist > curDist)
            {
                AddReward(reward / 5);
            }
            prevDist = curDist;
        }
    }
}
