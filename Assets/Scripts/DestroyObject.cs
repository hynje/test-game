using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    void Update()
    {
        if (transform.position.z < -20)
        {
            GameManager.Instance.DestroyGas(gameObject);
        }
    }
}
