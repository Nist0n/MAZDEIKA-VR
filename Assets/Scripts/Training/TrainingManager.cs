using System;
using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using TMPro;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    [SerializeField] private NPCConversation startingDialog;
    [SerializeField] private NPCConversation secondDialog;
    [SerializeField] private NPCConversation thirdDialog;
    [SerializeField] private TextMeshProUGUI baseAttackTrainingText;
    [SerializeField] private Animator textAnim;
    [SerializeField] private Animator baseAttackVideoAnimator;
    [SerializeField] private Animator shieldVideoAnimator;
    [SerializeField] private Animator breakShieldVideoAnimator;

    public bool FirstSkillTraining = false;
    public bool SecondSkillTraining = false;
    public bool ThirdSkillTraining = false;
    public int BaseAttackCompletedTimes;
    public int ShieldCompletedTimes;

    private bool _training1Completed = false;
    private bool _training2Completed = false;

    private TrainingEnemySkills _trainingEnemySkills;

    private void Start()
    {
        _trainingEnemySkills = FindObjectOfType<TrainingEnemySkills>();
        StartDialog(startingDialog);
    }

    private void Update()
    {
        BaseAttackCompletedTimes = Mathf.Clamp(BaseAttackCompletedTimes, 0, 5);
        ShieldCompletedTimes = Mathf.Clamp(ShieldCompletedTimes, 0, 5);
        if (!_training1Completed) CheckProgressCompletionBaseAttackTraining();
        if (!_training2Completed && _training1Completed) CheckProgressCompletionShieldTraining();
    }

    private void StartDialog(NPCConversation dialog)
    {
        ConversationManager.Instance.StartConversation(dialog);
    }

    private void CheckProgressCompletionBaseAttackTraining()
    {
        baseAttackTrainingText.color = Color.red;
        baseAttackTrainingText.text = $"Успешно совершено атак: {BaseAttackCompletedTimes}/5";
        if (BaseAttackCompletedTimes >= 5)
        {
            baseAttackTrainingText.color = Color.green;
            _training1Completed = true;
            textAnim.SetTrigger("hideText");
            baseAttackVideoAnimator.SetTrigger("hideVideo");
            StartDialog(secondDialog);
        }
    }
    
    private void CheckProgressCompletionShieldTraining()
    {
        baseAttackTrainingText.color = Color.red;
        baseAttackTrainingText.text = $"Успешно отражено атак: {ShieldCompletedTimes}/5";
        if (ShieldCompletedTimes >= 5)
        {
            baseAttackTrainingText.color = Color.green;
            _training2Completed = true;
            textAnim.SetTrigger("hideText");
            shieldVideoAnimator.SetTrigger("hideVideo");
            _trainingEnemySkills.enabled = false;
            StartDialog(thirdDialog);
        }
    }

    public void StartTraining1()
    {
        baseAttackTrainingText.gameObject.SetActive(true);
        FirstSkillTraining = true;
    }
    
    public void StartTraining2()
    {
        baseAttackTrainingText.gameObject.SetActive(true);
        SecondSkillTraining = true;
        _trainingEnemySkills.enabled = true;
    }
}
