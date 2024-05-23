using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttackSkill : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject hit;

    private bool isAttacking = false;
    private float _damage;
    
    private GameObject _enemy;
    private FirstEnemy _enemyClass;
    private PlayerController _player;
    void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _enemy = GameObject.FindGameObjectWithTag("Enemy");
        _enemyClass = FindObjectOfType<FirstEnemy>();
        _damage = _player.Damage;
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _enemy.transform.position, speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !isAttacking)
        {
            if (!_enemyClass.CanTakeDamage)
            {
                StartCoroutine(ShieldAttack());
            }
            else
            {
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        hit.SetActive(true);
        _player.GivenDamageToEnemyTimes++;
        _enemyClass.TakeDamage(_damage);
        AudioManager.instance.PlaySFX("BaseAttackImpact");
        yield return new WaitForSeconds(0.8f);
        isAttacking = false;
        Destroy(gameObject);
    }

    IEnumerator ShieldAttack()
    {
        isAttacking = true;
        hit.SetActive(true);
        AudioManager.instance.PlaySFX("ShieldImpact");
        yield return new WaitForSeconds(0.8f);
        isAttacking = false;
        Destroy(gameObject);
    }
}
