using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShieldDestroyer : MonoBehaviour
{
    private Enemy _enemy;

    private void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    private void Update()
    {
        if (_enemy.CanTakeDamage)
        {
            Destroy(gameObject);
        }
    }
}
