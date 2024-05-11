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
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _enemy.transform.position, speed);

        if (!_enemyClass.CanTakeDamage)
        {
            _damage = 0;
        }
        else
        {
            _damage = _player.Damage;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        hit.SetActive(true);
        _player.GivenDamageToEnemyTimes++;
        _enemyClass.TakeDamage(_damage);
        yield return new WaitForSeconds(0.8f);
        isAttacking = false;
        Destroy(gameObject);
    }
}
