using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySkills : MonoBehaviour
{
    [SerializeField] private GameObject baseAttackSkill;

    private bool _isAttacking = false;

    private float _time;
    private float _timer;

    private int _randomNumOfSkill;

    private void Start()
    {
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
    }

    private void CastSkill()
    {
        List<IEnumerator> functions = new List<IEnumerator>();
        functions.Add(BaseAttack());
        StartCoroutine(functions[_randomNumOfSkill]);
        _randomNumOfSkill = Random.Range(0, functions.Count - 1);
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
}
