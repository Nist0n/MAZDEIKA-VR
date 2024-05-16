using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skills : MonoBehaviour
{
    [SerializeField] private GameObject baseAttackSpell;
    [SerializeField] private GameObject breakShieldSkill;
    [SerializeField] private GameObject stunningAttackSkill;
    [SerializeField] private GameObject igniteSkill;
    [SerializeField] private Image igniteCd;
    [SerializeField] private Image stunCd;
    [SerializeField] private Image increaseDamageCd;
    [SerializeField] private Image healCd;
    [SerializeField] private Image defenceCd;

    private PlayerController _player;
    private FirstEnemy _enemy;

    private bool _canUseStun = true;
    private bool _canUseIgnite = true;
    private bool _canUseIncreaseDamage = true;
    private bool _canUseDefence = true;
    private bool _canUseHeal = true;
    private float _igniteCdTime = 8f;
    private float _igniteTimer;
    private float _stunCdTime = 7f;
    private float _stunTimer;
    private float _increaseDamageCdTime = 20f;
    private float _increaseDamageTimer;
    private float _healCdTime = 20f;
    private float _healTimer;
    private float _defenceCdTime = 10f;
    private float _defenceTimer;

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

        if (!_canUseIgnite)
        {
            _igniteTimer += Time.deltaTime;
            igniteCd.fillAmount = 1 - _igniteTimer / _igniteCdTime;
        }
        
        if (!_canUseStun)
        {
            _stunTimer += Time.deltaTime;
            stunCd.fillAmount = 1 - _stunTimer / _stunCdTime;
        }
        
        if (!_canUseIncreaseDamage)
        {
            _increaseDamageTimer += Time.deltaTime;
            increaseDamageCd.fillAmount = 1 - _increaseDamageTimer / _increaseDamageCdTime;
        }
        if (!_canUseHeal)
        {
            _healTimer += Time.deltaTime;
            healCd.fillAmount = 1 - _healTimer / _healCdTime;
        }
        if (!_canUseDefence)
        {
            _defenceTimer += Time.deltaTime;
            defenceCd.fillAmount = 1 - _defenceTimer / _defenceCdTime;
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
        if (SaveSystem.instance.firstEnemyDefeated)
        {
            if (_canUseStun)
            {
                Instantiate(stunningAttackSkill, transform);
                StartCoroutine(ActivateStunCD());
            }
        }
    }

    public void IgniteSkill()
    {
        if (SaveSystem.instance.firstEnemyDefeated)
        {
            if (_canUseIgnite)
            {
                Instantiate(igniteSkill, _enemy.gameObject.transform);
                StartCoroutine(ActivateIgniteCD());
            }
        }
    }

    public void IncreaseDamageSkill()
    {
        if (SaveSystem.instance.thirdEnemyDefeated)
        {
            if (_canUseIncreaseDamage)
            {
                StartCoroutine(_player.IncreaseDamage());
                StartCoroutine(ActivateIncreaseDamageCD());
            }
        }
    }

    public void HealSkill()
    {
        if (SaveSystem.instance.thirdEnemyDefeated)
        {
            if (_canUseHeal)
            {
                _player.RestoreHealth(100);
                StartCoroutine(ActivateHealCD());
            }
        }
    }

    public void CleanSkill()
    {
        if (SaveSystem.instance.secondEnemyDefeated)
        {
            _player.ActivateCleanSkill();
        }
    }

    public void DefenceSkill()
    {
        if (SaveSystem.instance.secondEnemyDefeated)
        {
            if (_canUseDefence)
            {
                StartCoroutine(_player.ActivateDefence());
                StartCoroutine(ActivateDefenceCD());
            }
        }
    }
    
    private IEnumerator ActivateStunCD()
    {
        _canUseStun = false;
        stunCd.gameObject.SetActive(true);
        yield return new WaitForSeconds(7f);
        stunCd.gameObject.SetActive(false);
        _canUseStun = true;
        _stunTimer = 0;
    }
    
    private IEnumerator ActivateIgniteCD()
    {
        _canUseIgnite = false;
        igniteCd.gameObject.SetActive(true);
        yield return new WaitForSeconds(_igniteCdTime);
        igniteCd.gameObject.SetActive(false);
        _canUseIgnite = true;
        _igniteTimer = 0;
    }
    
    private IEnumerator ActivateIncreaseDamageCD()
    {
        _canUseIncreaseDamage = false;
        increaseDamageCd.gameObject.SetActive(true);
        yield return new WaitForSeconds(_increaseDamageCdTime);
        increaseDamageCd.gameObject.SetActive(false);
        _canUseIncreaseDamage = true;
        _increaseDamageTimer = 0;
    }
    
    private IEnumerator ActivateHealCD()
    {
        _canUseHeal = false;
        healCd.gameObject.SetActive(true);
        yield return new WaitForSeconds(_healCdTime);
        healCd.gameObject.SetActive(false);
        _canUseHeal = true;
        _healTimer = 0;
    }
    
    private IEnumerator ActivateDefenceCD()
    {
        _canUseDefence = false;
        defenceCd.gameObject.SetActive(true);
        yield return new WaitForSeconds(_defenceCdTime);
        defenceCd.gameObject.SetActive(false);
        _canUseDefence = true;
        _defenceTimer = 0;
    }
}
