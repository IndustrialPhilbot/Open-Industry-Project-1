using UnityEngine;
using libplctag;
using libplctag.DataTypes;
using libplctag.NativeImport;
using System;
using System.Threading.Tasks;

public class PowerTurn : MonoBehaviour
{
    public bool enablePLC = false;
    public string tagName;
    public float speed = 1;
    public bool running = true;

    new readonly Tag<DintPlcMapper, int> tag = new();

    Rigidbody rb;
    Vector3 startAngle = new();
    Vector3 startPos = new();
    Vector3 centerOfMass = Vector3.zero;

    int scantime = 0;
    int failCount = 0;

    private void Start()
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
        startAngle = rb.transform.eulerAngles;
        startPos = rb.transform.position;
    }

    void Update()
    {
        if (running)
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
        try
        {
            speed = await tag.ReadAsync();
        }
        catch (Exception)
        {
            if (failCount > 0)
            {
                CancelInvoke(nameof(ScanTag));
                Debug.LogError($"Failed to read tag for object: {gameObject.name} check PLC object settings or Tag Name");
            }
            failCount++;
        }
    }
}
