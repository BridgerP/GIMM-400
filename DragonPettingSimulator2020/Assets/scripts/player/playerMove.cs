using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    public Camera cam;
    public CharacterController controller;
    public float speed;
    public float rotSpeed;
    public float gravSpeed;

    private Vector3 move;
    private Vector2 mouse;
    private float xRotation = 0f;
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        mouse.x = Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
        mouse.y = Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;
        
        xRotation -= mouse.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        move = transform.right * x + transform.forward * z;
        move.y = - gravSpeed;

        controller.Move(move * speed * Time.deltaTime);
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouse.x * rotSpeed * Time.deltaTime);
    }
}
