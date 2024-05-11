using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirdEnemySkills : MonoBehaviour
{
    [SerializeField] private GameObject baseAttackSkill;
    [SerializeField] private GameObject poisonSkill;
    [SerializeField] private GameObject stunSkill;
    [SerializeField] private Image skillImage;
    [SerializeField] private float defence;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject hand;

    private FirstEnemy _enemy;
    private PlayerController _player;

    private bool _isAttacking = false;
    private float _time;
    private float _timer;
    private int _randomNumOfSkill;
    private bool _canUsePoisonSkill = true;
    private float _poisonSkillCd = 7f;
    private float _poisonTimer = 0;
    private bool _canUseStunSkill = true;
    private float _stunSkillCd = 6f;
    private float _stunTimer = 0;
    private float _tempEnemyDamage;
    public bool IsIncreasedAttack = false;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _enemy = FindObjectOfType<FirstEnemy>();
        _randomNumOfSkill = 0;
        _time = Random.Range(2f, 3.5f);
        _enemy.UpdateDefence(defence);
        _tempEnemyDamage = _enemy.Damage;
    }

    void Update()
    {
        if (IsIncreasedAttack)
        {
            _randomNumOfSkill = 0;
        }
        
        CheckPoisonCd();
        
        CheckStunCd();
        
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

        if (_enemy.CurrentHealth <= 0)
        {
            _enemy.CanTakeDamage = false;
            gameObject.GetComponent<ThirdEnemySkills>().enabled = false;
        }
    }

    private void CastSkill()
    {
        {
            List<IEnumerator> functions = new List<IEnumerator>();
            functions.Add(BaseAttack());
            functions.Add(PoisonAttack());
            functions.Add(StunAttack());
            if (functions[_randomNumOfSkill].ToString().Contains("BaseAttack"))
            {
                StartCoroutine(functions[_randomNumOfSkill]);
                Debug.Log("BaseAttack");
            }
            if (functions[_randomNumOfSkill].ToString().Contains("PoisonAttack"))
            {
                if (_canUsePoisonSkill && !_player.IsPoisoned)
                {
                    StartCoroutine(functions[_randomNumOfSkill]);
                    Debug.Log("PoisonAttack");
                }
                else
                {
                    _randomNumOfSkill = Random.Range(0, functions.Count);
                    CastSkill();
                }
            }
            if (functions[_randomNumOfSkill].ToString().Contains("StunAttack"))
            {
                if (_canUseStunSkill && !_player.IsStunned)
                {
                    StartCoroutine(functions[_randomNumOfSkill]);
                    Debug.Log("StunAttack");
                }
                else
                {
                    _randomNumOfSkill = Random.Range(0, functions.Count);
                    CastSkill();
                }
            }
            
            _randomNumOfSkill = Random.Range(0, functions.Count);
        }
    }

    private void CheckPoisonCd()
    {
        if (!_canUsePoisonSkill)
        {
            _poisonTimer += Time.deltaTime;
            if (_poisonTimer >= _poisonSkillCd)
            {
                _canUsePoisonSkill = true;
                _poisonTimer = 0;
            }
        }
    }
    
    private void CheckStunCd()
    {
        if (!_canUseStunSkill)
        {
            _stunTimer += Time.deltaTime;
            if (_stunTimer >= _stunSkillCd)
            {
                _canUseStunSkill = true;
                _stunTimer = 0;
            }
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
        _time = Random.Range(2, 3.5f);
        _isAttacking = false;
        skillImage.color = new Color(255f, 255f, 255f, 0f);
    }

    IEnumerator PoisonAttack()
    {
        animator.SetTrigger("baseAttack");
        skillImage.sprite = poisonSkill.GetComponent<Image>().sprite;
        skillImage.color = new Color(255f, 255f, 255f, 255f);
        yield return new WaitForSeconds(1f);
        if (!_enemy.IsStunned)
        {
            Instantiate(poisonSkill, hand.transform.position, Quaternion.identity, _enemy.transform);
            _canUsePoisonSkill = false;
        }
        _timer = 0;
        _time = Random.Range(2, 3.5f);
        _isAttacking = false;
        skillImage.color = new Color(255f, 255f, 255f, 0f);
    }
    
    IEnumerator StunAttack()
    {
        animator.SetTrigger("baseAttack");
        skillImage.sprite = poisonSkill.GetComponent<Image>().sprite;
        skillImage.color = new Color(255f, 255f, 255f, 255f);
        yield return new WaitForSeconds(1f);
        if (!_enemy.IsStunned)
        {
            Instantiate(stunSkill, hand.transform.position, Quaternion.identity, _enemy.transform);
            _canUseStunSkill = false;
            _timer = 0;
            _time = Random.Range(2, 3.5f);
            _isAttacking = false;
            skillImage.color = new Color(255f, 255f, 255f, 0f);
        }
        else
        {
            _timer = 0;
            _time = Random.Range(2, 3.5f);
            _isAttacking = false;
            skillImage.color = new Color(255f, 255f, 255f, 0f);
        }
    }

    public void InstantinateBaseAttack()
    {
        Instantiate(baseAttackSkill, gameObject.transform);
    }

    public void InstantinatePoisonAttack()
    {
        Instantiate(poisonSkill, gameObject.transform);
        _canUsePoisonSkill = false;
    }

    public void InstantinateStunAttack()
    {
        Instantiate(stunSkill, gameObject.transform);
        _canUseStunSkill = false;
        _timer = 0;
        _time = Random.Range(2, 3.5f);
        _isAttacking = false;
    }
}
