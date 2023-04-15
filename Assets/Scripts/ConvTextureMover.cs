using UnityEngine;

public class ConvTextureMover : MonoBehaviour
{
    private MeshRenderer objectRenderer;
    private Conveyor _conveyor;
    private PowerTurn _powerTurn;
    private float convSpeed;
    private float conveyor_speed;
    private bool conveyor_running;
    private bool flipped;
    [SerializeField] private bool flipMaterialSlots;
    [SerializeField] private MeshRenderer[] convEnds;
    private int materialIndex = 0;

    private void Start()
    {
        Transform conveyor = transform.Find("Conveyor");
        objectRenderer = conveyor.GetComponent<MeshRenderer>();
        _conveyor = GetComponent<Conveyor>();
        _powerTurn = GetComponent<PowerTurn>();

        if (flipMaterialSlots) materialIndex = 1;
    }

    private void Update()
    {
        if (_conveyor != null)
        {
            conveyor_speed = _conveyor.speed;
            conveyor_running = _conveyor.run;

        }
        else
        {
            conveyor_speed = _powerTurn.speed;
            conveyor_running = _powerTurn.run;
        }

        if (conveyor_speed < 0 && !flipped)
        {
            FlipConveyor();
        }
        else if (conveyor_speed > 0 && flipped)
        {
            FlipConveyor();
        }
        if (conveyor_running)
        {
            if (!flipped)
            {
                convSpeed += -conveyor_speed * Time.deltaTime;
            }
            else
            {
                convSpeed += conveyor_speed * Time.deltaTime;
            }
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
