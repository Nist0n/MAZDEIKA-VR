using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgniteSkill : MonoBehaviour
{
    private bool _isAttacking = false;
    private float _damage;
    
    private FirstEnemy _enemyClass;
    private PlayerController _player;
    
    private void Start()
    {
        _enemyClass = FindObjectOfType<FirstEnemy>();
        _player = FindObjectOfType<PlayerController>();
        _damage = _player.IgniteDamage;
        
        if (!_isAttacking)
        {
            if (_enemyClass.CanTakeDamage && !_enemyClass.IsBurning)
            {
                StartCoroutine(Attack());
            }
            else
            {
                StartCoroutine(ShieldAttack());
            }
        }
    }

    IEnumerator Attack()
    {
        _isAttacking = true;
        _enemyClass.IsBurning = true;
        StartCoroutine(_enemyClass.ActivateTickDamage(_damage));
        StartCoroutine(_enemyClass.ActivateIgniteAura());
        yield return new WaitForSeconds(5f);
        _isAttacking = false;
        Destroy(gameObject);
    }
    
    IEnumerator ShieldAttack()
    {
        _isAttacking = true;
        yield return new WaitForSeconds(2f);
        _isAttacking = false;
        Destroy(gameObject);
    }
}
