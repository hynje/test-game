using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private GameManager gameManager;
    
    public float speed = 3f;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    
    void Update()
    {
        MoveCar();
    }

    void MoveCar()
    {
        if (Input.GetMouseButton(0) && gameManager.isGameActive)
        {
            Vector3 touchPos = Input.mousePosition;
            if (touchPos.x > Screen.width / 2)
            {
                transform.Translate(Vector3.right * Time.deltaTime * speed);
            }
            else
            {
                transform.Translate(Vector3.left * Time.deltaTime * speed);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        gameManager.GetGas();
    }
}
