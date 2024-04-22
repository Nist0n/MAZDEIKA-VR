using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySkills : MonoBehaviour
{
    [SerializeField] private GameObject baseAttackSkill;
    [SerializeField] private GameObject ultimateSkill;
    [SerializeField] private GameObject ultimateCharge;

    private Enemy _enemy;

    private bool _isAttacking = false;
    private float _time;
    private float _timer;
    private int _randomNumOfSkill;
    private bool _ultimateIsReady = false;

    private void Start()
    {
        _enemy = FindObjectOfType<Enemy>();
        _randomNumOfSkill = 0;
        _time = Random.Range(2, 4);
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_time - _timer <= 1.5f && !_isAttacking)
        {
            _isAttacking = true;
            CastSkill();
        }

        if (_enemy.CurrentHealth <= 300 && !_ultimateIsReady)
        {
            _ultimateIsReady = true;
        }
    }

    private void CastSkill()
    {
        if (!_ultimateIsReady)
        {
            List<IEnumerator> functions = new List<IEnumerator>();
            functions.Add(BaseAttack());
            StartCoroutine(functions[_randomNumOfSkill]);
            if (functions[_randomNumOfSkill].ToString().Contains("BaseAttack")) Debug.Log("BaseAttack");
            _randomNumOfSkill = Random.Range(0, functions.Count);
        }
        else
        {
            StartCoroutine(UltimateAttack());
        }
    }

    IEnumerator BaseAttack()
    {
        Debug.Log("Warning");
        yield return new WaitForSeconds(1.5f);
        Instantiate(baseAttackSkill, gameObject.transform);
        _timer = 0;
        _time = Random.Range(2, 4);
        _isAttacking = false;
    }
    
    IEnumerator UltimateAttack()
    {
        Debug.Log("ULT");
        Instantiate(ultimateCharge, gameObject.transform);
        yield return new WaitForSeconds(3f);
        Instantiate(ultimateSkill, gameObject.transform);
        _timer = 0;
        _time = Random.Range(2, 4);
        _isAttacking = true;
    }
}
