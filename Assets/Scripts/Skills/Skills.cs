using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    [SerializeField] private GameObject baseAttackSpell;
    [SerializeField] private GameObject breakShieldSkill;
    [SerializeField] private GameObject stunningAttackSkill;

    private PlayerController _player;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
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
        Instantiate(stunningAttackSkill, transform);
    }

}
