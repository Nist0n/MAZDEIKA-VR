using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttackSkill : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject hit;

    private bool isAttacking = false;
    private float _damage = 5;
    
    private GameObject _enemy;
    private Enemy _enemyClass;
    void Start()
    {
        _enemy = GameObject.FindGameObjectWithTag("Enemy");
        _enemyClass = _enemy.GetComponent<Enemy>();
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
            _damage = 5;
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
        _enemyClass.TakeDamage(_damage);
        yield return new WaitForSeconds(0.8f);
        isAttacking = false;
        Destroy(gameObject);
    }
}
