using UnityEngine;
using System.Threading.Tasks;
using System;
public class ifmLaserDistance : MonoBehaviour
{
    public bool enablePLC = false;
    public string tagName;
    public float distance = 10.0f;
    int value = 0;
    
    PLC plc;

    Guid id = Guid.NewGuid();
    void Start()
    {
        if (enablePLC)
        {
            plc = GameObject.Find("PLC").GetComponent<PLC>();
            plc.Connect(id, PLC.DataType.Int, tagName, gameObject);
            InvokeRepeating(nameof(ScanTag), 0, (float)plc.ScanTime / 1000f);
        }
    }
    void Update()
    {
        if (Physics.Raycast(transform.position + new Vector3(0, 0, 0), transform.TransformDirection(Vector3.forward), out RaycastHit hit, distance))
        {
            Debug.DrawRay(transform.position + new Vector3(0, 0, 0), transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            value = Convert.ToInt32(hit.distance * 1000); //Convert to mm
            //Debug.Log(tag.Value); 
        }
        else
        {
            Debug.DrawRay(transform.position + new Vector3(0, 0, 0), transform.TransformDirection(Vector3.forward) * distance, Color.red);
            value = Convert.ToInt32(distance * 1000); //Convert to mm
            //Debug.Log(tag.Value); 
        }
    }

    async Task ScanTag()
    {
        await plc.Write(id,value);
    }
}
