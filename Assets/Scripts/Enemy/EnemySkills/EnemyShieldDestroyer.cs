using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShieldDestroyer : MonoBehaviour
{
    private FirstEnemy _enemy;

    private void Start()
    {
        _enemy = FindObjectOfType<FirstEnemy>();
    }

    private void Update()
    {
        if (_enemy.CanTakeDamage)
        {
            Destroy(gameObject);
        }
    }
}
