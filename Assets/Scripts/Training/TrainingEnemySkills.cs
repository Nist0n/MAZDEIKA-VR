using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingEnemySkills : MonoBehaviour
{
    [SerializeField] private GameObject baseAttackSkill;
    [SerializeField] private Image skillImage;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject hand;

    private FirstEnemy _enemy;
    private PlayerController _player;

    private bool _isAttacking = false;
    private float _time;
    private float _timer;
    private int _randomNumOfSkill;

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
        if (_time - _timer <= 1f && !_isAttacking)
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
        if (functions[_randomNumOfSkill].ToString().Contains("BaseAttack")) Debug.Log("BaseAttack");
        _randomNumOfSkill = Random.Range(0, functions.Count);
    }

    IEnumerator BaseAttack()
    {
        animator.SetTrigger("baseAttack");
        skillImage.sprite = baseAttackSkill.GetComponent<Image>().sprite;
        skillImage.color = new Color(255f, 255f, 255f, 255f);
        yield return new WaitForSeconds(1f);
        Instantiate(baseAttackSkill, hand.transform.position, Quaternion.identity, _enemy.transform);
        AudioManager.instance.PlaySFX("BaseAttackCast");
        _timer = 0;
        _time = Random.Range(3, 4);
        _isAttacking = false;
        skillImage.color = new Color(255f, 255f, 255f, 0f);
    }
}
