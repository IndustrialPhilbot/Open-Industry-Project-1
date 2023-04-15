using System.Collections.Generic;
using UnityEngine;
public class ConvScaler : MonoBehaviour
{
    public bool isStraightConveyor;
    private MeshRenderer objectRenderer;
    private Conveyor _conveyor;
    private PowerTurn _powerTurn;
    private float conveyor_speed;
    private bool conveyor_running;
    private float convSpeed;
    [SerializeField] private Transform[] convEnds;
    public Transform rollers;
    [SerializeField] private Transform rollersUp;
    [SerializeField] private Transform rollersDown;
    [SerializeField] private GameObject rollerPrefab;
    public int rollerCount = 0;
    public float curConvSize = 0;
    private const float rollerSize = 0.333f;
    private List<MeshRenderer> rollerRenderers = new List<MeshRenderer>();
    public MeshRenderer convR;

    private void Start()
    {
        objectRenderer = GetComponent<MeshRenderer>();
        _conveyor = GetComponentInParent<Conveyor>();
        _powerTurn = GetComponentInParent<PowerTurn>();

        if (!isStraightConveyor)
        {
            AddMeshRenderers(rollers);
            if (rollersUp.gameObject.activeSelf) AddMeshRenderers(rollersUp);
        }
    }

    void AddMeshRenderers(Transform rollers)
    {
        for (int i = 0; i < rollers.childCount; i++)
        {
            rollerRenderers.Add(rollers.GetChild(i).GetComponent<MeshRenderer>());
        }
    }

    private void CreateRollers2()
    {
        float convXSize = transform.localScale.x;
        rollerCount = Mathf.RoundToInt(convXSize / rollerSize) + 1;
        if (rollers.childCount < rollerCount - 2 && rollerCount > 0)
        {
            Transform newRoller = Instantiate(rollerPrefab, rollers).transform;
            newRoller.localPosition = new Vector3(rollerCount * rollerSize - rollerSize * 2, 0.17f, 0);
        }
        else if (rollers.childCount > rollerCount - 2)
        {
            if (rollers.childCount > 2)
            {
                int lastRollerIndex = rollers.childCount - 1;
                Transform roller = rollers.GetChild(lastRollerIndex);
                DestroyImmediate(roller.gameObject, false);
            }
        }
        rollers.localScale = new Vector3(1f / transform.localScale.x, 1f, 1f);
        RollersPosition();
    }

    void RollersPosition()
    {
        float convXSize = transform.localScale.x;
        rollerCount = Mathf.RoundToInt(convXSize / rollerSize) + 1;
        for (int i = 0; i < rollers.childCount; i++)
        {
            rollers.GetChild(i).localPosition = new Vector3((i + 3) * rollerSize - rollerSize * 2, 0.17f, 0);
        }
    }

    void GetRollers()
    {
        RemoveRollers();
    }

    void RemoveRollers()
    {
        for (int i = 0; i < rollers.childCount; i++)
        {
            Destroy(rollers.GetChild(i).gameObject);
        }
        curConvSize = 0;
    }

    private void MoveRollers()
    {
        if (isStraightConveyor)
        {
            for (int i = 0; i < rollers.childCount; i++)
            {
                RotateRoller(rollers.GetChild(i));
            }
            foreach (Transform endRoller in convEnds)
            {
                RotateRoller(endRoller.GetChild(0));
            }
        }
        else
        {
            foreach (var renderer in rollerRenderers)
            {
                renderer.materials[0].mainTextureOffset = new Vector2(convSpeed * 10f, 0);
            }
        }
    }
    
    private void RotateRoller(Transform roller)
    {
        float rotationSpeed = 0;
        if (conveyor_running) rotationSpeed = conveyor_speed * Time.deltaTime;
        else rotationSpeed = 0;
        if (roller.parent.localEulerAngles.y != 0) rotationSpeed = -rotationSpeed;
        roller.Rotate(Vector3.forward, rotationSpeed * 220f);
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

        if (conveyor_running)
        {
            convSpeed += conveyor_speed * Time.deltaTime;
        }
        else
        {
            convSpeed = 0;
        }

        MoveRollers();
    }

    void ScaleRollers(Transform rollers)
    {
        for (int i = 0; i < rollers.childCount; i++)
        {
            rollers.GetChild(i).localScale = new Vector3(1f / Mathf.Clamp(transform.localScale.x, 0, 1000), 1f, 1f);
        }
    }

    void ScaleAngleEnds()
    {
        convEnds[0].localScale = new Vector3(1f / Mathf.Clamp(transform.localScale.x, 1, 1000) , 1f, 1f);
        convEnds[1].localScale = new Vector3(1f , 1f, 1f / Mathf.Clamp(transform.localScale.z, 1, 1000));
    }

    void AddAdditionalRollers()
    {
        if (!rollersUp.gameObject.activeSelf)
        {
            rollersUp.gameObject.SetActive(true);
        }
    }

    void ChangeRollers(bool down)
    {
        if (down && !rollersDown.gameObject.activeSelf)
        {
            rollersDown.gameObject.SetActive(true);
            rollers.gameObject.SetActive(false);
        }
        else if (!down && !rollers.gameObject.activeSelf)
        {
            rollers.gameObject.SetActive(true);
            rollersDown.gameObject.SetActive(false);
        }
    }

    void RemoveAdditionalRollers()
    {
        if (rollersUp.gameObject.activeSelf)
        {
            rollersUp.gameObject.SetActive(false);
        }
    }

    public void RemakeObject()
    {
        if(isStraightConveyor) transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x, 1f, 1000f), 1, transform.localScale.z);
        else transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x, 0f, 1000f), 1, Mathf.Clamp(transform.localScale.z, 0f, 1000f));
        foreach (var convEnd in convEnds)
        {
            if(isStraightConveyor) convEnd.localScale = new Vector3(1f / transform.localScale.x , 1f, 1f);
        }
        if(isStraightConveyor) CreateRollers2();
        else
        {
            ScaleRollers(rollers);
            ScaleRollers(rollersUp);
            ScaleRollers(rollersDown);
            ScaleAngleEnds();
            if (transform.localScale.x >= 2 && transform.localScale.z >= 2) AddAdditionalRollers();
            else if (transform.localScale.x < 2 && transform.localScale.z < 2) RemoveAdditionalRollers();
            
            if(transform.localScale.x < 1 && transform.localScale.z < 1) ChangeRollers(true);
            else if(transform.localScale.x >= 1 && transform.localScale.z >= 1) ChangeRollers(false);
        }
    }
}
