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
        
        objectRenderer = GetComponent<MeshRenderer>();
        _conveyor = GetComponentInParent<Conveyor>();
        _powerTurn = GetComponentInParent<PowerTurn>();

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
        Transform parentTransform = GetComponentInParent<Transform>();
        if (this.gameObject.GetComponentInChildren<PowerTurn>() != null)
        {
            parentTransform.localScale = new Vector3(Mathf.Clamp(parentTransform.localScale.x, 0.51f, 1000f), 1, Mathf.Clamp(parentTransform.localScale.z, 0.51f, 1000f));
        }
        else
        {
            parentTransform.localScale = new Vector3(Mathf.Clamp(parentTransform.localScale.x, 0.51f, 1000f), 1, parentTransform.localScale.z);
        }
        foreach (var convEnd in convEnds)
        {
            convEnd.transform.localScale = new Vector3(1f / parentTransform.lossyScale.x, 1f, 1f);
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
