using UnityEngine;
using System.Threading.Tasks;
using System;

public class ifmDiffuseSensor : MonoBehaviour
{
    public bool enablePLC = false;
    public string tagName;
    public float distance = 6.0f;
    
    Transform childTransform;

    sbyte value = 0;

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

        childTransform = GetComponentInChildren<Transform>();
    }
    void Update()
    {
        if (Physics.Raycast(childTransform.position + new Vector3(0, 0.16f, 0), childTransform.TransformDirection(Vector3.forward), out RaycastHit hit, distance))
        {
            Debug.DrawRay(childTransform.position + new Vector3(0, 0.16f, 0), childTransform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            value = 1;
        }
        else
        {
            Debug.DrawRay(childTransform.position + new Vector3(0, 0.16f, 0), childTransform.TransformDirection(Vector3.forward) * distance, Color.red);
            value = 0;
        }
    }

    async Task ScanTag()
    {
        await plc.Write(id,value);
    }
}