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
        _time = Random.Range(3f, 3.5f);
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

        if (_enemy.CurrentHealth <= 0 || _player.CurrentHealth <= 0)
        {
            _enemy.CanTakeDamage = false;
            gameObject.GetComponent<ThirdEnemySkills>().enabled = false;
        }
        
        if (_enemy.CurrentHealth <= 0)
        {
            SaveSystem.instance.thirdEnemyDefeated = true;
            SaveSystem.instance.Save();
        }
        
        if (_player.CurrentHealth <= 0)
        {
            FirstDefeat();
        }
    }

    private void CastSkill()
    {
        {
            List<IEnumerator> functions = new List<IEnumerator>();
            
            functions.Add(BaseAttack());
            if (_canUsePoisonSkill) functions.Add(PoisonAttack());
            if (_canUseStunSkill) functions.Add(StunAttack());

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
                    _canUsePoisonSkill = false;
                    Debug.Log("PoisonAttack");
                }
                else
                {
                    _randomNumOfSkill = Random.Range(0, functions.Count);
                    CastSkill();
                    return;
                }
            }
            
            if (functions[_randomNumOfSkill].ToString().Contains("StunAttack"))
            {
                if (_canUseStunSkill && !_player.IsStunned)
                {
                    StartCoroutine(functions[_randomNumOfSkill]);
                    _canUseStunSkill = false;
                    Debug.Log("StunAttack");
                }
                else
                {
                    _randomNumOfSkill = Random.Range(0, functions.Count);
                    CastSkill();
                    return;
                }
            }

            if (!_canUsePoisonSkill)
            {
                var temp = functions;
                foreach (var b in temp)
                {
                    if (b.ToString().Contains("PoisonAttack"))
                    {
                        functions.Remove(b);
                        Debug.Log("PoisonAttack Deleted");
                        break;
                    }
                }
            }
            
            if (!_canUseStunSkill)
            {
                var temp = functions;
                foreach (var b in temp)
                {
                    if (b.ToString().Contains("StunAttack"))
                    {
                        functions.Remove(b);
                        Debug.Log("StunAttack Deleted");
                        break;
                    }
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
            AudioManager.instance.PlaySFX("BaseAttackCast");
            Instantiate(baseAttackSkill, hand.transform.position, Quaternion.identity, _enemy.transform);
        }
        _timer = 0;
        _time = Random.Range(3, 3.5f);
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
            AudioManager.instance.PlaySFX("PoisonCast");
            Instantiate(poisonSkill, hand.transform.position, Quaternion.identity, _enemy.transform);
        }
        _timer = 0;
        _time = Random.Range(3, 3.5f);
        _isAttacking = false;
        skillImage.color = new Color(255f, 255f, 255f, 0f);
    }
    
    IEnumerator StunAttack()
    {
        animator.SetTrigger("baseAttack");
        skillImage.sprite = stunSkill.GetComponent<Image>().sprite;
        skillImage.color = new Color(255f, 255f, 255f, 255f);
        yield return new WaitForSeconds(1f);
        if (!_enemy.IsStunned)
        {
            AudioManager.instance.PlaySFX("StunAttackCast");
            Instantiate(stunSkill, hand.transform.position, Quaternion.identity, _enemy.transform);
        }
        _timer = 0;
        _time = Random.Range(3, 3.5f);
        _isAttacking = false;
        skillImage.color = new Color(255f, 255f, 255f, 0f);
    }
    
    private void FirstDefeat()
    {
        if (AchieveAchievement.instance.CompleteAchievement("FirstDefeat") == false)
        {
            AchieveAchievement.instance.SetBoolParamToAchievement("FirstDefeat");
        }

        SaveSystem.instance.Save();
    }
}
