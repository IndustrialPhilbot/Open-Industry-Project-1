using UnityEngine;
using System.Threading.Tasks;
using System;
using Unity.VisualScripting;

[SelectionBase]
public class Conveyor : MonoBehaviour
{
    public bool enablePLC = false;
    public string tagName;
    public float speed = 0;
    public bool run = false;

    PLC plc;

    Vector3 startPos = new();
    Rigidbody rb;

    Guid id = Guid.NewGuid();
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();

        startPos = rb.transform.position;

        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }
    void Update()
    {

        if (IsInvoking(nameof(ScanTag)) && !enablePLC)
        {
            CancelInvoke(nameof(ScanTag));
        }
        else if(!IsInvoking(nameof(ScanTag)) && enablePLC)
        {
            plc = GameObject.Find("PLC").GetComponent<PLC>();
            plc.Connect(id, PLC.DataType.Float, tagName, gameObject);
            InvokeRepeating(nameof(ScanTag), 0, (float)plc.ScanTime / 1000f);
        }

        if (run)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            rb.velocity = transform.TransformDirection(Vector3.left) * speed;
            rb.transform.position = startPos;
        }
        else
        {
            if(rb.velocity != Vector3.zero && rb.velocity.y == 0)
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
        try
        {
            speed = await plc.ReadFloat(id);
        }
        catch(Exception ex)
        {
            Debug.LogException(ex);
        }
        
    }
}
