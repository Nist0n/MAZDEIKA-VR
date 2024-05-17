using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttackTraining : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject hit;

    private bool isAttacking = false;
    private float _damage;
    
    private GameObject _enemy;
    private TrainingManager _trainingManager;
    
    void Start()
    {
        _trainingManager = FindObjectOfType<TrainingManager>();
        _enemy = GameObject.FindGameObjectWithTag("Enemy");
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _enemy.transform.position, speed);
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
        _trainingManager.BaseAttackCompletedTimes++;
        yield return new WaitForSeconds(0.8f);
        isAttacking = false;
        Destroy(gameObject);
    }
}
