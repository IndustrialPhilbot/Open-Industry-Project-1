using UnityEngine;

public class ConvTextureMover : MonoBehaviour
{
    private MeshRenderer objectRenderer;
    private Conveyor _conveyor;
    private float convSpeed;
    private bool flipped;
    [SerializeField] private bool flipMaterialSlots;
    [SerializeField] private MeshRenderer[] convEnds;
    private int materialIndex = 0;

    private void Start()
    {
        Transform conveyor = transform.Find("Conveyor");
        objectRenderer = conveyor.GetComponent<MeshRenderer>();
        _conveyor = GetComponent<Conveyor>();
        if (flipMaterialSlots) materialIndex = 1;
    }

    private void Update()
    {
        if (_conveyor != null)
        {
            if (_conveyor.speed < 0 && !flipped)
            {
                FlipConveyor();
            }
            else if (_conveyor.speed > 0 && flipped)
            {
                FlipConveyor();
            }
            if (_conveyor.conveyorRunning)
            {
                if (!flipped)
                {
                    convSpeed += -_conveyor.speed * Time.deltaTime;
                }
                else
                {
                    convSpeed += _conveyor.speed * Time.deltaTime;
                }
            }
        }
        else
        {
            convSpeed -= Time.deltaTime;
        }

        objectRenderer.materials[materialIndex].mainTextureOffset = new Vector2(0, convSpeed);
        foreach (var convEnd in convEnds)
        {
            convEnd.materials[0].mainTextureOffset = new Vector2(0, convSpeed);
        }
    }

    public void RemakeMesh()
    {
        if(this.gameObject.GetComponentInChildren<PowerTurn>() != null)
        {
            transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x, 0.51f, 1000f), 1, Mathf.Clamp(transform.localScale.z, 0.51f, 1000f));
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x, 0.51f, 1000f), 1, transform.localScale.z);
        }
        foreach (var convEnd in convEnds)
        {
            convEnd.transform.localScale = new Vector3(1f / transform.localScale.x, 1f, 1f);
        }
    }

    void FlipConveyor()
    {
        flipped = !flipped;
        float flipAngle = 0;
        if (flipped) flipAngle = 180f;
        objectRenderer.materials[materialIndex].SetFloat("_TextureAngle", flipAngle);
        foreach (var convEnd in convEnds)
        {
            convEnd.materials[0].SetFloat("_TextureAngle", flipAngle);
        }
    }
}
