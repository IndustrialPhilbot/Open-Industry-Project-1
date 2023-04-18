using libplctag.DataTypes;
using libplctag;
using UnityEngine;
using libplctag.NativeImport;
using System.Threading.Tasks;
using System;
using Unity.VisualScripting;

public class Photoeye : MonoBehaviour
{
    new readonly Tag<SintPlcMapper, sbyte> tag = new();
    public bool enablePLC = false;
    public string tagName;
    public float distance = 6.0f;

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
    }
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out RaycastHit hit, distance))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);
            value = 1;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * distance, Color.red);
            value = 0;
        }
    }

    async Task ScanTag()
    {
        await plc.Write(id, value);
    }
}
