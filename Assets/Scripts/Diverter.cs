using UnityEngine;
using System.Threading.Tasks;
using System;

public class Diverter : MonoBehaviour
{
    public bool enablePLC = true;

    Rigidbody rb;

    float time = 0;

    public string tagName;

    public bool fireDivert = false;
    public float divertTime = 0;
    public float divertSpeed = 0;

    Vector3 startPos = new();

    bool divert = false;
    bool cycled = false;

    PLC plc;

    Guid id = Guid.NewGuid();

    // Start is called before the first frame update
    void Start()
    {
        if (enablePLC)
        {
            plc = GameObject.Find("PLC").GetComponent<PLC>();
            plc.Connect(id, PLC.DataType.Bool, tagName, gameObject);
            InvokeRepeating(nameof(ScanTag), 0, (float)plc.ScanTime / 1000f);
        }
        
        //Set new rigidbody
        rb = GetComponentInChildren<DiverterAnimator>().GetDiverterRigidbody();

        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!divert && startPos != transform.position)
        {
            startPos = transform.position;
        }

        if(fireDivert && !cycled)
        {
            //This is manual testing
            fireDivert = false;
            divert= true;
        }
        else if (fireDivert == false)
        {
            cycled = false;
        }

        if (divert && !cycled)
        {
            time += Time.deltaTime;

            if (time < divertTime)
            {
                rb.velocity = Vector3.forward * divertSpeed;
            }
            else
            {

                rb.velocity = Vector3.back * divertSpeed;

                if (time > divertTime * 2)
                {
                    rb.velocity = Vector3.zero;
                    transform.position = startPos;
                    time = 0;
                    divert = false;
                    cycled = true;
                }
            }
        }

    }

    async Task ScanTag()
    {
        fireDivert = await plc.ReadBool(id);
    }

}
