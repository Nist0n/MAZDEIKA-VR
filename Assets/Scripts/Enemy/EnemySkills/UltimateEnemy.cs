using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateEnemy : MonoBehaviour
{
    private GameObject _player;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        transform.LookAt(_player.transform);
    }
}
