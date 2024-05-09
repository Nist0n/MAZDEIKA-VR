using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondEnemyScript : MonoBehaviour
{
    [SerializeField] private GameObject baseAttackSkill;
    [SerializeField] private Image skillImage;
    [SerializeField] private float defence;

    private FirstEnemy _enemy;

    private bool _isAttacking = false;
    private float _time;
    private float _timer;
    private int _randomNumOfSkill;

    private void Start()
    {
        _enemy = FindObjectOfType<FirstEnemy>();
        _randomNumOfSkill = 0;
        _time = Random.Range(1.5f, 3);
        _enemy.UpdateDefence(defence);
    }

    void Update()
    {
        if (!_enemy.IsStunned)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            _timer = 0;
        }

        if (_time - _timer <= 1.5f && !_isAttacking)
        {
            _isAttacking = true;
            CastSkill();
        }

        if (_enemy.CurrentHealth <= 0)
        {
            _enemy.CanTakeDamage = false;
            gameObject.GetComponent<SecondEnemyScript>().enabled = false;
        }
    }

    private void CastSkill()
    {
        {
            List<IEnumerator> functions = new List<IEnumerator>();
            functions.Add(BaseAttack());
            StartCoroutine(functions[_randomNumOfSkill]);
            if (functions[_randomNumOfSkill].ToString().Contains("BaseAttack")) Debug.Log("BaseAttack");
            _randomNumOfSkill = Random.Range(0, functions.Count);
        }
    }

    IEnumerator BaseAttack()
    {
        skillImage.sprite = baseAttackSkill.GetComponent<Image>().sprite;
        skillImage.color = new Color(255f, 255f, 255f, 255f);
        yield return new WaitForSeconds(1.5f);
        if (!_enemy.IsStunned)
        {
            Instantiate(baseAttackSkill, gameObject.transform);
        }
        _timer = 0;
        _time = Random.Range(1.5f, 3);
        _isAttacking = false;
        skillImage.color = new Color(255f, 255f, 255f, 0f);
    }
}
