using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    [SerializeField] private GameObject baseAttackSpell;
    [SerializeField] private GameObject breakShieldSkill;

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
    }

    public void BaseAttack()
    {
        Instantiate(baseAttackSpell, transform);
    }

    public void BreakShield()
    {
        Instantiate(breakShieldSkill, transform);
    }

}
