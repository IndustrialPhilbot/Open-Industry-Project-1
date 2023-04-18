using UnityEngine;
using System.Threading.Tasks;
using System;

public class RetExtConveyor : MonoBehaviour
{
    public bool Extend;
    public bool retract;
    public float ExtendSize;
    public float retractSize;

    public bool enablePLC = false;
    public string tagName;
    public float speed;

    float moveTime = 0.0f;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Extend)
        {
            moveTime += 1f * Time.deltaTime;
            if (moveTime >= 1 || transform.localScale.x >= ExtendSize)
            {
                transform.localScale = new Vector3(ExtendSize, 1f, 1f);
                Extend = false;
                moveTime = 0;
            }
            else
            {
                transform.localScale = new Vector3(Mathf.Lerp(retractSize, ExtendSize, moveTime), 1f, 1f);
            }


        }

        if (retract)
        {
            moveTime += 1f * Time.deltaTime;
            if (moveTime >= 1 || transform.localScale.x <= retractSize)
            {
                transform.localScale = new Vector3(retractSize, 1f, 1f);
                retract = false;
                moveTime = 0;
            }
            else
            {
                transform.localScale = new Vector3(Mathf.Lerp(ExtendSize, retractSize, moveTime), 1f, 1f);
            }
        }
    }

    async Task ScanTag()
    {
        speed = await plc.ReadFloat(id);
    }
}
