using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour
{ 
    [SerializeField] private float _speed = 4f;
    
    void Update()
    {
        if (GameManager.Instance.isGameActive)
        {
            transform.Translate(-Vector3.forward * Time.deltaTime * _speed);
        }
    }
}
