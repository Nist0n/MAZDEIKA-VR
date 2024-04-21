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
    private Enemy _enemy;
    void Start()
    {
        _enemy = FindObjectOfType<Enemy>();
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
            _damage = _enemy.Damage;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        hit.SetActive(true);
        _playerController.TakeDamage(_damage);
        yield return new WaitForSeconds(0.8f);
        isAttacking = false;
        Destroy(gameObject);
    }
}