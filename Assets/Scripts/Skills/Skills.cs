using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    [SerializeField] private GameObject baseAttackSpell;
    [SerializeField] private GameObject breakShieldSkill;
    [SerializeField] private GameObject stunningAttackSkill;
    [SerializeField] private GameObject igniteSkill;

    private PlayerController _player;
    private FirstEnemy _enemy;

    private bool _canUseStun = true;
    private bool _canUseIgnite = true;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _enemy = FindObjectOfType<FirstEnemy>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            BaseAttack();
        }
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            BreakShield();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ActivateShield();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            StunningAttack();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            IgniteSkill();
        }
    }

    public void BaseAttack()
    {
        Instantiate(baseAttackSpell, transform);
    }

    public void BreakShield()
    {
        Instantiate(breakShieldSkill, transform);
    }

    public void ActivateShield()
    {
        StartCoroutine(_player.ActivateShield());
    }

    public void StunningAttack()
    {
        if (_canUseStun)
        {
            Instantiate(stunningAttackSkill, transform);
            StartCoroutine(ActivateStunCD());
        }
    }

    public void IgniteSkill()
    {
        if (_canUseIgnite)
        {
            Instantiate(igniteSkill, _enemy.gameObject.transform);
            StartCoroutine(ActivateIgniteCD());
        }
    }

    private IEnumerator ActivateStunCD()
    {
        _canUseStun = false;
        yield return new WaitForSeconds(7f);
        _canUseStun = true;
    }
    
    private IEnumerator ActivateIgniteCD()
    {
        _canUseIgnite = false;
        yield return new WaitForSeconds(8f);
        _canUseIgnite = true;
    }

}
