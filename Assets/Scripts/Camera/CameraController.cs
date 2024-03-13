using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private int _sensitivity = 200;

    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        transform.eulerAngles += _sensitivity * new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0f);
    }
}
