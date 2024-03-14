using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private int _damage;
    private int _currentHealth;

    private void Start()
    {
        _currentHealth = _health;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth < 0)
        {
            Destroy(gameObject);
        }
    }
}
