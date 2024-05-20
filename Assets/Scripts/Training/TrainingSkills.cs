using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingSkills : MonoBehaviour
{
    [SerializeField] private GameObject baseAttackSpell;
    [SerializeField] private GameObject breakShieldSkill;
    [SerializeField] private GameObject handPos;

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
        if (_trainingManager.FirstSkillTraining) Instantiate(baseAttackSpell, handPos.transform.position, Quaternion.identity, _player.transform);
    }

    public void BreakShield()
    {
        if (_trainingManager.ThirdSkillTraining) Instantiate(breakShieldSkill, handPos.transform.position, Quaternion.identity, _player.transform);
    }

    public void ActivateShield()
    {
        if (_trainingManager.SecondSkillTraining) StartCoroutine(_player.ActivateShield());
    }
}
