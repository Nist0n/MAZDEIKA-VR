using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float _power;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Weapon"))
        {
            return;
        }

        rb.isKinematic = false;
        rb.AddForce(Vector3.up * _power);
    }
}
