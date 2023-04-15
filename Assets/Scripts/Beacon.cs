using libplctag;
using libplctag.DataTypes;
using libplctag.NativeImport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Beacon : MonoBehaviour
{
    public bool enablePLC = false;
    public string tagName;
    public bool lightBeacon = false;
    new readonly Tag<SintPlcMapper, sbyte> tag = new();
    Material material;

    PLC plc;
    void Start()
    {
        if (enablePLC)
        {
            plc = GameObject.Find("PLC").GetComponent<PLC>();
            plc.Connect(tagName, 1, gameObject);
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
        lightBeacon = Convert.ToBoolean(await plc.Read(gameObject));
    }
}
