using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;

    [SerializeField] private float _spawnInterval;

    [SerializeField] private int _minX;
    [SerializeField] private int _maxX;
    [SerializeField] private int _minZ;
    [SerializeField] private int _maxZ;

    [SerializeField] private float _height;

    private float _currentSpawnTimer;

    private void Update()
    {
        _currentSpawnTimer += Time.deltaTime;

        if (_currentSpawnTimer >= _spawnInterval)
        {
            var enemyInstance = Instantiate(_enemyPrefab);

            var newPosition = GenerateStartPosition();

            enemyInstance.transform.position = newPosition;

            _currentSpawnTimer = 0;
        }
    }

    private Vector3 GenerateStartPosition()
    {
        var startPos = new Vector3(Random.Range(_minX, _maxX), _height, Random.Range(_minZ,_maxZ));

        return startPos;
    }
}
