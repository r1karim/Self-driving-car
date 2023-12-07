using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Integrations.Match3;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed = 20f;
    public float turnSpeed = 42;
    public float HorizontalInput;
    public float VerticalInput;
    public bool running = true;
    private Rigidbody rb;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody>();       
    }

    public void start()
    {
        running = true;
    }
    public void stop()
    {
        running = false;
    }

    public void setInput(float vertical, float horizontal)
    {
        HorizontalInput = horizontal;
        VerticalInput = vertical;
    }
    void FixedUpdate()
    {


        //HorizontalInput = Input.GetAxis("Horizontal");
        //VerticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * Time.deltaTime * speed * VerticalInput);

        //rb.MovePosition(Vector3.forward * Time.deltaTime * speed * VerticalInput);
        //transform.position += Vector3.forward * Time.deltaTime * speed * VerticalInput;
        transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed * HorizontalInput);
     
    }
}
