using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondEnemyScript : MonoBehaviour
{
    [SerializeField] private GameObject baseAttackSkill;
    [SerializeField] private Image skillImage;
    [SerializeField] private float defence;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject hand;

    private FirstEnemy _enemy;
    private PlayerController _playerController;

    private bool _isAttacking = false;
    private float _time;
    private float _timer;
    private int _randomNumOfSkill;

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
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

        if (_time - _timer <= 1f && !_isAttacking)
        {
            _isAttacking = true;
            CastSkill();
        }

        if (_enemy.CurrentHealth <= 0 || _playerController.CurrentHealth <= 0)
        {
            _enemy.CanTakeDamage = false;
            gameObject.GetComponent<SecondEnemyScript>().enabled = false;
        }

        if (_enemy.CurrentHealth <= 0)
        {
            SaveSystem.instance.secondEnemyDefeated = true;
            SaveSystem.instance.Save();
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
        animator.SetTrigger("baseAttack");
        skillImage.sprite = baseAttackSkill.GetComponent<Image>().sprite;
        skillImage.color = new Color(255f, 255f, 255f, 255f);
        yield return new WaitForSeconds(1f);
        if (!_enemy.IsStunned)
        {
            Instantiate(baseAttackSkill, hand.transform.position, Quaternion.identity, _enemy.transform);
        }
        _timer = 0;
        _time = Random.Range(1.5f, 3);
        _isAttacking = false;
        skillImage.color = new Color(255f, 255f, 255f, 0f);
    }
}
