using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttackEnemySkill : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject hit;

    private bool isAttacking = false;
    private float _damage;

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

        if (!_playerController.CanTakeDamage)
        {
            _damage = 0;
        }
        else
        {
            if ((FindObjectOfType<ThirdEnemySkills>()!= null && FindObjectOfType<ThirdEnemySkills>().IsIncreasedAttack) || (FindObjectOfType<FourthEnemyScills>()!= null && FindObjectOfType<FourthEnemyScills>().IsIncreasedAttack))
            {
                _damage = _enemy.Damage * 2;
            }
            else
            {
                _damage = _enemy.Damage;
            }
        }
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
        _playerController.TakeDamage(_damage);
        AudioManager.instance.PlaySFX("BaseAttackImpact");
        yield return new WaitForSeconds(0.4f);
        if (FindObjectOfType<ThirdEnemySkills>() != null && FindObjectOfType<ThirdEnemySkills>().IsIncreasedAttack)
            FindObjectOfType<ThirdEnemySkills>().IsIncreasedAttack = false;
        if (FindObjectOfType<FourthEnemyScills>() != null && FindObjectOfType<FourthEnemyScills>().IsIncreasedAttack)
            FindObjectOfType<FourthEnemyScills>().IsIncreasedAttack = false;
        isAttacking = false;
        Destroy(gameObject);
    }
    
    IEnumerator ShieldAttack()
    {
        isAttacking = true;
        hit.SetActive(true);
        AudioManager.instance.PlaySFX("ShieldImpact");
        yield return new WaitForSeconds(0.4f);
        if (FindObjectOfType<ThirdEnemySkills>() != null && FindObjectOfType<ThirdEnemySkills>().IsIncreasedAttack)
            FindObjectOfType<ThirdEnemySkills>().IsIncreasedAttack = false;
        if (FindObjectOfType<FourthEnemyScills>() != null && FindObjectOfType<FourthEnemyScills>().IsIncreasedAttack)
            FindObjectOfType<FourthEnemyScills>().IsIncreasedAttack = false;
        isAttacking = false;
        Destroy(gameObject);
    }
}
