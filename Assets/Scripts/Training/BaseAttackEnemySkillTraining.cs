using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttackEnemySkillTraining : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject hit;

    private bool isAttacking = false;

    private GameObject _player;
    private TrainingManager _trainingManager;
    private PlayerController _playerScript;
    void Start()
    {
        _trainingManager = FindObjectOfType<TrainingManager>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerScript = _player.GetComponent<PlayerController>();
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isAttacking)
        {
            if (_playerScript.CanTakeDamage)
            {
                StartCoroutine(Attack());
            }
            else
            {
                StartCoroutine(ShieldAttack());
            }
        }
    }

    IEnumerator ShieldAttack()
    {
        isAttacking = true;
        hit.SetActive(true);
        AudioManager.instance.PlaySFX("ShieldImpact");
        _trainingManager.ShieldCompletedTimes++;
        yield return new WaitForSeconds(0.4f);
        isAttacking = false;
        Destroy(gameObject);
    }
    
    IEnumerator Attack()
    {
        isAttacking = true;
        hit.SetActive(true);
        AudioManager.instance.PlaySFX("BaseAttackImpact");
        yield return new WaitForSeconds(0.8f);
        isAttacking = false;
        Destroy(gameObject);
    }
}
