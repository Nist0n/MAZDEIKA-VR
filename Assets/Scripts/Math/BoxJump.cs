using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class BoxJump : MonoBehaviour
{
    protected float animation;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animation += Time.deltaTime;

        animation = animation % 5f;

        transform.position = MathParabola.Parabola(UnityEngine.Vector3.zero, UnityEngine.Vector3.forward * 10f, 5f,
            animation / 5f);
    }
}
