using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate_around_point : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.Find("Complete_Limit_Lever/Root/Piviot").position, Vector3.up, 20 * Time.deltaTime);
    }
}
