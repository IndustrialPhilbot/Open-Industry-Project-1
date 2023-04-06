using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerTurn : MonoBehaviour
{
    public bool running = true;
    public float speed = 1f;

    Rigidbody rb;
    Vector3 startAngle = new();
    Vector3 startPos = new();
    Vector3 centerOfMass = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startAngle = transform.eulerAngles;
        startPos = transform.position;
    }

    void Update()
    {
        //test
        if (running)
        {
            rb.centerOfMass = centerOfMass;
            rb.WakeUp();
            rb.angularVelocity = transform.TransformDirection(Vector3.up) * speed;
            transform.eulerAngles = startAngle;
            transform.position = startPos;
        }
        else
        {
            if (transform.eulerAngles != Vector3.zero)
            {
                transform.position = startPos;
                transform.eulerAngles = startAngle;
                rb.angularVelocity = Vector3.zero;
            }
            startPos = transform.position;
            startAngle = transform.eulerAngles;
        }
    }
}
