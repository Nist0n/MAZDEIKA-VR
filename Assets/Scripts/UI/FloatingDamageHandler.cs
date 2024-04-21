using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamageHandler : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 1.15f);
        transform.localPosition = new Vector3(0f, 0.5f, 0f);
    }
}
