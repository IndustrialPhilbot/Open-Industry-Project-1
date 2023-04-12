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
    public string tagName;
    public float distance = 6.0f;
    int scanTime = 0;

    void Start()
    {
        try { plctag.ForceExtractLibrary = false; } catch { };

        var _plc = GameObject.Find("PLC").GetComponent<PLC>();

        tag.Name = tagName;
        tag.Gateway = _plc.Gateway;
        tag.Path = _plc.Path;
        tag.PlcType = _plc.PlcType;
        tag.Protocol = _plc.Protocol;
        tag.Timeout = TimeSpan.FromSeconds(1);

        scanTime = _plc.ScanTime;

        try
        {
            InvokeRepeating(nameof(ScanTag), 0, (float)scanTime / 1000f);
        }
        catch (Exception)
        {
            Debug.LogError($"Failed to write to tag for object: {gameObject.name} check PLC object settings or Tag Name");
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
        await tag.WriteAsync();
    }
}