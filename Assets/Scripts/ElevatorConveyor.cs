using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorConveyor : MonoBehaviour
{

    private Conveyor _conveyor;

    public bool moveUp = false;
    public bool moveDown = false;
    public float upPos = 10f;
    public float downPos = 0f;

    public float upSpeed = 5f;
    public float downSpeed = 5f;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        _conveyor = GetComponent<Conveyor>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!_conveyor.run)
        {
            if (moveUp)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX;
                rb.velocity = Vector3.up * upSpeed;
                if (transform.position.y >= upPos)
                {
                    moveUp = false;
                    rb.velocity = Vector3.zero;
                    transform.position = new Vector3(transform.position.x, upPos, transform.position.z);
                    rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
                }
            }

            if (moveDown)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX;
                rb.velocity = -Vector3.up * downSpeed;
                if (transform.position.y <= downPos)
                {
                    moveDown = false;
                    rb.velocity = Vector3.zero;
                    transform.position = new Vector3(transform.position.x, downPos, transform.position.z);
                    rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
                }
            }
        }
        else
        {
            moveDown = false;
            moveUp = false;
        }
    }
}
