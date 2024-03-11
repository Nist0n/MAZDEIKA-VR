using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRActivation : MonoBehaviour
{
    private ParabolaController _parabolaController;
    private bool _isFirst = true;
    
    private void Start()
    {
        _parabolaController = FindObjectOfType<ParabolaController>();
    }

    private void Update()
    {
        if (!_parabolaController.Animation && _isFirst)
        {
            Enable();
            _isFirst = false;
        }
    }

    private void Enable()
    {
        transform.LeanScale(Vector3.one, 0.8f);
    }
}
