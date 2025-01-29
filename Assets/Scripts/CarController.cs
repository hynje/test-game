using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    public int maxGas = 100;

    public void Move(float direction)
    {
        transform.Translate(Vector3.right * (direction * moveSpeed * Time.deltaTime));
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -2f, 2f), 0, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Gas")) return;
        GameManager.Instance.DestroyGas(other.gameObject);
        GameManager.Instance.GetGas();
    }
}
