using System;
using System.Threading.Tasks;
using UnityEngine;

public class Beacon : MonoBehaviour
{
    public bool enablePLC = false;
    public string tagName;
    public bool lightBeacon = false;
    Material material;

    PLC plc;

    Guid id = Guid.NewGuid();
    void Start()
    {
        if (enablePLC)
        {
            plc = GameObject.Find("PLC").GetComponent<PLC>();
            plc.Connect(id, PLC.DataType.Bool, tagName,gameObject);
            InvokeRepeating(nameof(ScanTag), 0, (float)plc.ScanTime / 1000f);
        }

        material = GetComponent<MeshRenderer>().materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (lightBeacon)
        {
            material.EnableKeyword("_EMISSION");
        }
        else
        {
            material.DisableKeyword("_EMISSION");
        }
    }

    async Task ScanTag()
    {
        lightBeacon = await plc.ReadBool(id);
    }
}
