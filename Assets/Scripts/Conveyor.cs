using UnityEngine;
using libplctag;
using libplctag.DataTypes;
using libplctag.NativeImport; 
using System;
using System.Threading.Tasks;


public class Conveyor : MonoBehaviour
{
    public bool enablePLC = false;
    public string tagName;
    public float speed = 1;
    public bool running = false;

    new readonly Tag<DintPlcMapper, int> tag = new();

    Vector3 startPos = new();
    Rigidbody rb;

    int scantime = 0;
    int failCount = 0;


    void Start()
    {
        if (enablePLC)
        {
            try { plctag.ForceExtractLibrary = false; } catch { };

            var _plc = GameObject.Find("PLC").GetComponent<PLC>();

            tag.Name = tagName;
            tag.Gateway = _plc.Gateway;
            tag.Path = _plc.Path;
            tag.PlcType = _plc.PlcType;
            tag.Protocol = _plc.Protocol;
            tag.Timeout = TimeSpan.FromSeconds(5);

            scantime = _plc.ScanTime;

            InvokeRepeating(nameof(ScanTag), 0, (float)scantime / 1000f);
        }

        rb = GetComponentInChildren<Rigidbody>();

        startPos = rb.transform.position;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }
    void Update()
    {
        if (running)
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
            speed = await tag.ReadAsync();
        }
        catch (Exception)
        {
            if(failCount > 0)
            {
                CancelInvoke(nameof(ScanTag));
                Debug.LogError($"Failed to read tag for object: {gameObject.name} check PLC object settings or Tag Name");
            }
            failCount++;
        }
    }
}
