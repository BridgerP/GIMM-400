using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drive : MonoBehaviour
{
    Rigidbody rb;

    public float brakeSpeed;
    public float maxSpeed;
    public float speed;
    public float rotSpeed;

    float velocity;
    float rotation;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.isKinematic = true;
    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.S) && velocity > 10) velocity /= 1 + brakeSpeed;
        else if (Input.GetKey(KeyCode.S) && velocity > 2) velocity /= 1 + brakeSpeed * 1.5f;
        else if (Input.GetKey(KeyCode.S) && velocity > .1) velocity /= 1 + brakeSpeed * 2;
        else if (Input.GetKey(KeyCode.S)) velocity = -5f;
        else if (Input.GetKey(KeyCode.W))
        {
            if (Mathf.Abs(rb.velocity.z) < maxSpeed)
                velocity += speed;
        }
        else velocity /= 1 + brakeSpeed / 4;

        if (velocity > 20)
        {
            if (Input.GetKey(KeyCode.A)) rotation -= rotSpeed / (Mathf.Abs(velocity) / 15);
            if (Input.GetKey(KeyCode.D)) rotation += rotSpeed / (Mathf.Abs(velocity) / 15);
        }
        else if (velocity > 7)
        {
            if (Input.GetKey(KeyCode.A)) rotation -= rotSpeed / (Mathf.Abs(velocity) / 5);
            if (Input.GetKey(KeyCode.D)) rotation += rotSpeed / (Mathf.Abs(velocity) / 5);
        }
        else if (velocity != 0)
        {
            if (Input.GetKey(KeyCode.A)) rotation -= 1;
            if (Input.GetKey(KeyCode.D)) rotation += 1;
        }
        else if (velocity < 0)
        {
            if (Input.GetKey(KeyCode.A)) rotation += 1;
            if (Input.GetKey(KeyCode.D)) rotation -= 1;
        }

        if (velocity > maxSpeed) velocity = 38;
        if (velocity - Mathf.Abs(rb.velocity.magnitude) > 1) velocity = Mathf.Abs((velocity + rb.velocity.magnitude) / 2);

        rb.velocity = transform.forward * velocity;
        transform.rotation = Quaternion.Euler(0, rotation, 0);
    }
}
