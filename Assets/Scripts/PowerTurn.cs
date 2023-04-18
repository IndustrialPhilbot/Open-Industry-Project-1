using UnityEngine;
using System.Threading.Tasks;
using System;

public class PowerTurn : MonoBehaviour
{
    public bool enablePLC = false;
    public string tagName;
    public float speed = 0;
    public bool run = true;

    Rigidbody rb;
    Vector3 startAngle = new();
    Vector3 startPos = new();
    Vector3 centerOfMass = Vector3.zero;

    PLC plc;

    Guid id = Guid.NewGuid();
    private void Start()
    {
        if (enablePLC)
        {
            plc = GameObject.Find("PLC").GetComponent<PLC>();
            plc.Connect(id, PLC.DataType.Bool, tagName, gameObject);
            InvokeRepeating(nameof(ScanTag), 0, (float)plc.ScanTime / 1000f);
        }

        rb = GetComponentInChildren<Rigidbody>();
        startAngle = rb.transform.eulerAngles;
        startPos = rb.transform.position;
    }

    void Update()
    {
        if (run)
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

    async Task ScanTag()
    {
        speed = await plc.ReadFloat(id);
    }
}
