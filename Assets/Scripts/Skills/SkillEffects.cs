using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillEffects : MonoBehaviour
{
    [SerializeField] private Image defenceCd;
    [SerializeField] private Image stunCd;
    [SerializeField] private Image increaseDamageCd;
    
    private float _stunCdTime = 5f;
    private float _stunTimer;
    private float _defenceCdTime = 7f;
    private float _defenceTimer;
    private float _increaseDamageCdTime = 10f;
    private float _increaseDamageTimer;
    
    private void Update()
    {
        if (gameObject.name.Contains("Stun")) StunTimer();
        if (gameObject.name.Contains("Defence")) DefenceTimer();
        if (gameObject.name.Contains("IncreaseDamage")) IncreaseDamageTimer();
    }

    private void DefenceTimer()
    {
        _defenceTimer += Time.deltaTime;
        defenceCd.fillAmount = _defenceTimer / _defenceCdTime;
    }
    
    private void StunTimer()
    {
        _stunTimer += Time.deltaTime;
        stunCd.fillAmount = _stunTimer / _stunCdTime;
    }
    
    private void IncreaseDamageTimer()
    {
        _increaseDamageTimer += Time.deltaTime;
        increaseDamageCd.fillAmount = _increaseDamageTimer / _increaseDamageCdTime;
    }
}
