using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingSkills : MonoBehaviour
{
    [SerializeField] private GameObject baseAttackSpell;
    [SerializeField] private GameObject breakShieldSkill;

    private PlayerController _player;
    private FirstEnemy _enemy;
    private TrainingManager _trainingManager;
    
    

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _enemy = FindObjectOfType<FirstEnemy>();
        _trainingManager = FindObjectOfType<TrainingManager>();
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
    }

    public void BaseAttack()
    {
        if (_trainingManager.FirstSkillTraining) Instantiate(baseAttackSpell, transform);
    }

    public void BreakShield()
    {
        if (_trainingManager.ThirdSkillTraining) Instantiate(breakShieldSkill, transform);
    }

    public void ActivateShield()
    {
        if (_trainingManager.SecondSkillTraining) StartCoroutine(_player.ActivateShield());
    }
}
