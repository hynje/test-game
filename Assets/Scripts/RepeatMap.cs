using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatMap : MonoBehaviour
{
    public GameObject road;
    void Update()
    {
        if (transform.position.z < -11)
        {
            transform.position = road.transform.position + new Vector3(0, 0, 10);
        }
    }
}
