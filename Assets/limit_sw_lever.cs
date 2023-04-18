using System;
using System.Threading.Tasks;
using UnityEngine;


public class limit_sw_lever : MonoBehaviour
{
    public bool enablePLC = false;
    public string tagName;

    private float angleFeedback;

    Transform moveTransform;
    Transform rotateTransform;
    HingeJoint rotateHinge;
    JointSpring hingeSpring;
    JointLimits limits;

    sbyte value = 0;

    PLC plc;
    Guid id = Guid.NewGuid();

    void Start()
    {
        moveTransform   = transform.Find("Complete_Limit_Lever/Root/Body/Pivot/Lever-Rotate/Lever-Roller").GetComponent<Transform>();
        rotateTransform = transform.Find("Complete_Limit_Lever/Root/Body/Pivot/Lever-Rotate").GetComponent<Transform>();
        rotateHinge     = transform.Find("Complete_Limit_Lever/Root/Body/Pivot/Lever-Rotate").GetComponent<HingeJoint>();

        if (enablePLC)
        {
            plc = GameObject.Find("PLC").GetComponent<PLC>();
            plc.Connect(id,PLC.DataType.Bool,tagName,gameObject);
            InvokeRepeating(nameof(ScanTag), 0, (float)plc.ScanTime / 1000f);
        }
    }

    void Update()
    {
        angleFeedback = rotateTransform.localEulerAngles.z; 
        angleFeedback = (angleFeedback > 180) ? angleFeedback - 360 : angleFeedback; //Return Negative Angles
        //Debug.Log(angleFeedback); 

        if (angleFeedback < -1 || angleFeedback > 1)
        {
            value = 1;
            rotateTransform.Rotate(new Vector3(0, 0, 0)); //Make sure lever returns to setpoint after spring and damper effect
        }
        else
        {
            value = 0;
            rotateTransform.Rotate(new Vector3(0, 0, 0)); //Make sure lever returns to setpoint after spring and damper effect
        }
        //Debug.Log(value);
    }

    async Task ScanTag()
    {
        await plc.Write(id,value);
    }
}
