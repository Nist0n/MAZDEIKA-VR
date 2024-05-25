using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetMenu : MonoBehaviour
{
    void FixedUpdate()
    {
        gameObject.transform.rotation = Camera.main.transform.rotation;

        gameObject.transform.position = Camera.main.transform.position;
    }
}
