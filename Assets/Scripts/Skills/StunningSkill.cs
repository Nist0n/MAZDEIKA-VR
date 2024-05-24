using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunningSkill : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject hit;

    private bool isAttacking = false;

    private GameObject _enemy;
    private FirstEnemy _enemyClass;
    private PlayerController _player;
    void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _enemy = GameObject.FindGameObjectWithTag("Enemy");
        _enemyClass = _enemy.GetComponent<FirstEnemy>();
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _enemy.transform.position, speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !isAttacking)
        {
            if (_enemyClass.CanTakeDamage && !_enemyClass.IsStunned)
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
        isAttacking = true;
        hit.SetActive(true);
        AudioManager.instance.PlaySFX("StunAttackImpact");
        if (!_enemyClass.IsHealing) StartCoroutine(_enemyClass.ActivateStunEffect());
        else
        {
            _enemyClass.IsHealing = false;
            _enemy.GetComponentInChildren<Animator>().SetBool("isHealing", false);
        }
        yield return new WaitForSeconds(3f);
        isAttacking = false;
        Destroy(gameObject);
    }

    IEnumerator ShieldAttack()
    {
        isAttacking = true;
        hit.SetActive(true);
        AudioManager.instance.PlaySFX("ShieldImpact");
        yield return new WaitForSeconds(0.7f);
        isAttacking = false;
        Destroy(gameObject);
    }
}
