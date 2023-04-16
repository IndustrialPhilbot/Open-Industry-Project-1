using System;
using System.Threading.Tasks;
using UnityEngine;

public class Triple_Stack_Light : MonoBehaviour
{
    public bool enablePLC = false;
    public string tagSegment1;
    public string tagSegment2;
    public string tagSegment3;
    public bool segment1 = false;
    public bool segment2 = false;
    public bool segment3 = false;
    public Color segment1Color = new Color(0, 1.0f, 0.0f, 0.0f);
    public Color segment2Color = new Color(1.0f, 0.64f, 0.0f, 0.0f);
    public Color segment3Color = new Color(1.0f, 0.0f, 0.0f, 0.0f);

    Material materialSeg1;
    Material materialSeg2;
    Material materialSeg3;

    PLC plc;
    void Start()
    {
        if (enablePLC)
        {
            plc = GameObject.Find("PLC").GetComponent<PLC>();
            plc.Connect(tagSegment1, 1, gameObject);
            plc.Connect(tagSegment2, 1, gameObject);
            plc.Connect(tagSegment3, 1, gameObject);
            InvokeRepeating(nameof(ScanTag), 0, (float)plc.ScanTime / 1000f);
        }

        materialSeg1 = GameObject.Find("Tripple_Stack_Light/Root/color-seg-1").GetComponent<MeshRenderer>().material;
        materialSeg2 = GameObject.Find("Tripple_Stack_Light/Root/color-seg-2").GetComponent<MeshRenderer>().material;
        materialSeg3 = GameObject.Find("Tripple_Stack_Light/Root/color-seg-3").GetComponent<MeshRenderer>().material;
 
    }

    // Update is called once per frame
    void Update()
    {
        //Segment 1
        if (segment1)
        {
            materialSeg1.color = segment1Color;
            materialSeg1.SetColor("_EmissionColor", segment1Color * Mathf.Pow(2.0F, 3.5f));
            materialSeg1.EnableKeyword("_EMISSION");
        }
        else
        {
            materialSeg1.color = Color.white;
            materialSeg1.DisableKeyword("_EMISSION");
        }

        //Segment 2
        if (segment2)
        {
            materialSeg2.color = segment2Color;
            materialSeg2.SetColor("_EmissionColor", segment2Color * Mathf.Pow(2.0F, 3.5f));
            materialSeg2.EnableKeyword("_EMISSION");
        }
        else
        {
            materialSeg2.color = Color.white;
            materialSeg2.DisableKeyword("_EMISSION");
        }

        //Segment 3
        if (segment3)
        {
            materialSeg3.color = segment3Color;
            materialSeg3.SetColor("_EmissionColor", segment3Color * Mathf.Pow(2.0F, 3.5f));
            materialSeg3.EnableKeyword("_EMISSION");
        }
        else
        {
            materialSeg3.color = Color.white;
            materialSeg3.DisableKeyword("_EMISSION");
        }

    }

    async Task ScanTag()
    {
        segment1 = Convert.ToBoolean(await plc.Read(gameObject));
        segment2 = Convert.ToBoolean(await plc.Read(gameObject));
        segment3 = Convert.ToBoolean(await plc.Read(gameObject));
    }
}

