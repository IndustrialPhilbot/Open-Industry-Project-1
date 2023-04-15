using libplctag.DataTypes;
using libplctag;
using UnityEngine;
using libplctag.NativeImport;
using System.Threading.Tasks;
using System;
using Unity.VisualScripting;

public class ifmDiffuseSensor : MonoBehaviour
{
    new readonly Tag<SintPlcMapper, sbyte> tag = new();
    public bool enablePLC = false;
    public string tagName;
    public float distance = 6.0f;

    PLC plc;
    void Start()
    {
        if (enablePLC)
        {
            plc = GameObject.Find("PLC").GetComponent<PLC>();
            plc.Connect(tagName, 1, gameObject);
            InvokeRepeating(nameof(ScanTag), 0, (float)plc.ScanTime / 1000f);
        }
    }
    void Update()
    {
        if (Physics.Raycast(transform.position + new Vector3(0, 0.16f, 0), transform.TransformDirection(Vector3.forward), out RaycastHit hit, distance))
        {
            Debug.DrawRay(transform.position + new Vector3(0, 0.16f, 0), transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            tag.Value = 1;
        }
        else
        {
            Debug.DrawRay(transform.position + new Vector3(0, 0.16f, 0), transform.TransformDirection(Vector3.forward) * distance, Color.red);
            tag.Value = 0;
        }
    }

    async Task ScanTag()
    {
        await plc.Read(gameObject);
    }
}