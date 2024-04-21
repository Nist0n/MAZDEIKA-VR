using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakShieldSkill : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject hit;

    private bool _isAttacking = false;

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !_isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        _isAttacking = true;
        hit.SetActive(true);
        _enemyClass.DeActivateShield();
        yield return new WaitForSeconds(0.8f);
        _isAttacking = false;
        Destroy(gameObject);
    }
}
