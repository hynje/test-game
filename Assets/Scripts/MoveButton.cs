using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveButton : MonoBehaviour
{
    public delegate void MoveButtonDelegate();
    public event MoveButtonDelegate OnMoveButtonPressed;
    
    private bool _isDown = false;

    private void Update()
    {
        if (_isDown)
        {
            OnMoveButtonPressed?.Invoke();
        }
    }

    public void ButtonDown()
    {
        _isDown = true;
    }

    public void ButtonUp()
    {
        _isDown = false;
    }
}
