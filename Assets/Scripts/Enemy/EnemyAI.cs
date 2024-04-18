using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float _minWalkableDistance;
    [SerializeField] private float _maxWalkableDistance;

    [SerializeField] private float _reachedPointDistance;

    [SerializeField] private GameObject _roamTarget;

    [SerializeField] private float _targetFollowRange;
    [SerializeField] private EnemyAttack _enemyAttack;

    [SerializeField] private float _stopTargetFollowingRange;

    [SerializeField] private AIDestinationSetter _aiDestionationSetter;

    private PlayerController _player;

    private EnemyStates _currentStates;
    private Vector3 _roamPosition;

    private void Start()
    {

        _player = FindObjectOfType<PlayerController>();

        _currentStates = EnemyStates.Roaming;

        _roamPosition = GenerateRoamPosition;
    }
}
