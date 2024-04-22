using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateEnemyShard : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private float _damage;
    private bool _isAttacking = false;

    private GameObject _player;
    private PlayerController _playerController;
    private FirstEnemy _enemy;
    void Start()
    {
        _enemy = FindObjectOfType<FirstEnemy>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>();
        _damage = _enemy.Damage;
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, speed);

        if (_playerController.CurrentHealth == 0)
        {
            Destroy(gameObject.GetComponentInParent<UltimateEnemy>().gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !_isAttacking)
        {
            Debug.Log("DaMAGE");
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        Debug.Log("DaMAGE");
        _isAttacking = true;
        _playerController.TakeDamage(_damage);
        yield return new WaitForSeconds(0.15f);
        _isAttacking = false;
        StartCoroutine(Attack());
    }
}
