using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakShieldSkillTraining : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject hit;

    private bool _isAttacking = false;

    private GameObject _enemy;
    private FirstEnemy _enemyClass;
    private TrainingManager _trainingManager;
    private void Start()
    {
        _enemy = GameObject.FindGameObjectWithTag("Enemy");
        _enemyClass = _enemy.GetComponent<FirstEnemy>();
        _trainingManager = FindObjectOfType<TrainingManager>();
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _enemy.transform.position, speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !_isAttacking && !_enemyClass.CanTakeDamage)
        {
            StartCoroutine(Attack());
        }
        else
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                StartCoroutine(CantAttack());
            }
        }
    }

    IEnumerator Attack()
    {
        _isAttacking = true;
        hit.SetActive(true);
        _enemyClass.DeActivateShield();
        AudioManager.instance.PlaySFX("ShieldBroken");
        _trainingManager.BreakShieldCompletedTimes++;
        yield return new WaitForSeconds(0.8f);
        _isAttacking = false;
        Destroy(gameObject);
    }
    
    IEnumerator CantAttack()
    {
        _isAttacking = true;
        hit.SetActive(true);
        AudioManager.instance.PlaySFX("BaseAttackImpact");
        yield return new WaitForSeconds(0.8f);
        _isAttacking = false;
        Destroy(gameObject);
    }
}
