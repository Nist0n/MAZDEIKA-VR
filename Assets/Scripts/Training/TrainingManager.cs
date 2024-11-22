using System;
using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TrainingManager : MonoBehaviour
{
    [SerializeField] private NPCConversation startingDialog;
    [SerializeField] private NPCConversation secondDialog;
    [SerializeField] private NPCConversation thirdDialog;
    [SerializeField] private NPCConversation fourthDialog;
    [SerializeField] private TextMeshProUGUI baseAttackTrainingText;
    [SerializeField] private Animator textAnim;
    [SerializeField] private Animator baseAttackVideoAnimator;
    [SerializeField] private Animator shieldVideoAnimator;
    [SerializeField] private Animator breakShieldVideoAnimator;
    [SerializeField] private XRInteractorLineVisual _XRInteractorLineVisual;

    public bool FirstSkillTraining = false;
    public bool SecondSkillTraining = false;
    public bool ThirdSkillTraining = false;
    public int BaseAttackCompletedTimes;
    public int ShieldCompletedTimes;
    public int BreakShieldCompletedTimes;
    public bool TrainingIsOver = false;

    private bool _training1Completed = false;
    private bool _training2Completed = false;
    private bool _training3Completed = false;

    private TrainingEnemySkills _trainingEnemySkills;

    private void Start()
    {
        _XRInteractorLineVisual.enabled = true;
        _trainingEnemySkills = FindObjectOfType<TrainingEnemySkills>();
        StartDialog(startingDialog);
    }

    private void Update()
    {
        BaseAttackCompletedTimes = Mathf.Clamp(BaseAttackCompletedTimes, 0, 5);
        ShieldCompletedTimes = Mathf.Clamp(ShieldCompletedTimes, 0, 5);
        BreakShieldCompletedTimes = Mathf.Clamp(BreakShieldCompletedTimes, 0, 3);
        if (!_training1Completed) CheckProgressCompletionBaseAttackTraining();
        if (!_training2Completed && _training1Completed) CheckProgressCompletionShieldTraining();
        if (!_training3Completed && _training1Completed && _training2Completed) CheckProgressCompletionBreakShieldTraining();
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
    
    private void CheckProgressCompletionBreakShieldTraining()
    {
        baseAttackTrainingText.color = Color.red;
        baseAttackTrainingText.text = $"Успешно пробит щит: {BreakShieldCompletedTimes}/3";
        if (BreakShieldCompletedTimes >= 3)
        {
            baseAttackTrainingText.color = Color.green;
            _training3Completed = true;
            textAnim.SetTrigger("hideText");
            breakShieldVideoAnimator.SetTrigger("hideVideo");
            StartDialog(fourthDialog);
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
    
    public void StartTraining3()
    {
        baseAttackTrainingText.gameObject.SetActive(true);
        ThirdSkillTraining = true;
    }

    public void StartBattle()
    {
        _XRInteractorLineVisual.enabled = false;
        Debug.Log(_XRInteractorLineVisual.enabled);
        AudioManager.instance.PlayMusic("BattleMusic");
        TrainingIsOver = true;
        FindObjectOfType<FirstEnemySkills>().enabled = true;
        FindObjectOfType<TrainingSkills>().enabled = false;
        FindObjectOfType<Skills>().enabled = true;
    }
}
