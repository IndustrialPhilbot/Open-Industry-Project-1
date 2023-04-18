using UnityEngine;
using System.Threading.Tasks;
using System;

public class SkewedConveyor : MonoBehaviour
{
    public bool enablePLC = false;
    public string tagName;
    public float speed = 0;
    public float skewAngle = 0;
    public bool running = false;

    Vector3 startPos = new();
    Rigidbody rb;

    PLC plc;

    Guid id = Guid.NewGuid();
    void Start()
    {
        if (enablePLC)
        {
            plc = GameObject.Find("PLC").GetComponent<PLC>();
            plc.Connect(id, PLC.DataType.Bool, tagName, gameObject);
            InvokeRepeating(nameof(ScanTag), 0, (float)plc.ScanTime / 1000f);
        }

        rb = GetComponentInChildren<Rigidbody>();

        startPos = rb.transform.position;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }
    void Update()
    {

        //Get a scalar value derieved from the skew angle and the speed
        float skewScalar = Mathf.Sin(skewAngle * Mathf.Deg2Rad) * speed;

        if (running)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            rb.velocity = transform.TransformDirection(new Vector3(-1, 0, skewScalar)) * speed;
            rb.transform.position = startPos;
        }
        else
        {
            if (rb.velocity != Vector3.zero && rb.velocity.y == 0)
            {
                rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
                rb.transform.position = startPos;
                rb.velocity = Vector3.zero;
            }
            //when you stop, keep updating startPos for elevator
            startPos = transform.position;
        }
    }

    async Task ScanTag()
    {
        speed = await plc.ReadFloat(id);
    }
}
