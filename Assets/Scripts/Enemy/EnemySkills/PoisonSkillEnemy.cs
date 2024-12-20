using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSkillEnemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject hit;

    private bool isAttacking = false;

    private GameObject _player;
    private PlayerController _playerController;
    private FirstEnemy _enemy;
    void Start()
    {
        _enemy = FindObjectOfType<FirstEnemy>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>();
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isAttacking)
        {
            if (_playerController.CanTakeDamage)
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
        AudioManager.instance.PlaySFX("PoisonImpact");
        _playerController.PoisonPlayer(_enemy.PoisonDamage);
        yield return new WaitForSeconds(0.4f);
        isAttacking = false;
        Destroy(gameObject);
    }
    
    IEnumerator ShieldAttack()
    {
        isAttacking = true;
        hit.SetActive(true);
        AudioManager.instance.PlaySFX("ShieldImpact");
        yield return new WaitForSeconds(0.2f);
        AchieveAchievement.instance.SumBeatOffSpells();
        isAttacking = false;
        Destroy(gameObject);
    }
}
