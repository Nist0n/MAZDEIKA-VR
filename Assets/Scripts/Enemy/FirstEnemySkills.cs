using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FirstEnemySkills : MonoBehaviour
{
    [SerializeField] private GameObject baseAttackSkill;
    [SerializeField] private GameObject ultimateSkill;
    [SerializeField] private GameObject ultimateCharge;
    [SerializeField] private Image skillImage;

    private FirstEnemy _enemy;
    private PlayerController _player;

    private bool _isAttacking = false;
    private float _time;
    private float _timer;
    private int _randomNumOfSkill;
    private bool _ultimateIsReady = false;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _enemy = FindObjectOfType<FirstEnemy>();
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

        if ((_enemy.CurrentHealth <= 300 || _player.CurrentHealth <= 300) && !_ultimateIsReady)
        {
            _ultimateIsReady = true;
            _enemy.CanTakeDamage = false;
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
        skillImage.sprite = baseAttackSkill.GetComponent<Image>().sprite;
        skillImage.color = new Color(255f, 255f, 255f, 255f);
        yield return new WaitForSeconds(1.5f);
        Instantiate(baseAttackSkill, gameObject.transform);
        _timer = 0;
        _time = Random.Range(2, 4);
        _isAttacking = false;
        skillImage.color = new Color(255f, 255f, 255f, 0f);
    }
    
    IEnumerator UltimateAttack()
    {
        skillImage.sprite = ultimateCharge.GetComponent<Image>().sprite;
        skillImage.color = new Color(255f, 255f, 255f, 255f);
        Instantiate(ultimateCharge, gameObject.transform);
        yield return new WaitForSeconds(3f);
        Instantiate(ultimateSkill, gameObject.transform);
        _timer = 0;
        _time = Random.Range(2, 4);
        _isAttacking = true;
        skillImage.color = new Color(255f, 255f, 255f, 0f);
    }
}
