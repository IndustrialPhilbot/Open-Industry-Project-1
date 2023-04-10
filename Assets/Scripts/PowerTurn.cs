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
        rb = GetComponentInChildren<Rigidbody>();
        startAngle = rb.transform.eulerAngles;
        startPos = rb.transform.position;
    }

    void Update()
    {
        if (running)
        {
            rb.centerOfMass = centerOfMass;
            rb.WakeUp();
            rb.angularVelocity = transform.TransformDirection(Vector3.up) * speed;
            rb.transform.eulerAngles = startAngle;
            rb.transform.position = startPos;
        }
        else
        {
            if (transform.eulerAngles != Vector3.zero)
            {
                rb.transform.position = startPos;
                rb.transform.eulerAngles = startAngle;
                rb.angularVelocity = Vector3.zero;
            }
            startPos = rb.transform.position;
            startAngle = rb.transform.eulerAngles;
        }
    }
}
