using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyUnits : MonoBehaviour
{
    [SerializeField] private List<Enemy> _enemyUnit = new(3);
    [SerializeField] private TimeManager _timeManager = null;
    [SerializeField] private Transform[] _patrolPoints = null;
    [SerializeField] private float _waitTime = 3f;

    private float _timeVariable = 1f;
    void Awake()
    {
        if(_timeManager != null)
        {
            _timeManager.OnEnemyTimeChange += ChangeTimeVariable;
        }


        if(_patrolPoints != null)
        {
            foreach(Enemy enemy in _enemyUnit)
            {
                enemy.SetPatrolPoints(_patrolPoints);
                enemy.OnEnemyKilled += OnEnemyDestroyed;
            }
        }

        StartCoroutine(RandomPatrolEnemy());
    }

    private void OnDisable()
    {
        if(_timeManager != null)
        {
            _timeManager.OnEnemyTimeChange -= ChangeTimeVariable;
        }
    }
    private void OnEnemyDestroyed(Enemy enemy)
    {
        enemy.OnEnemyKilled -= OnEnemyDestroyed;
        _enemyUnit.Remove(enemy);
    }
    private IEnumerator RandomPatrolEnemy()
    {
        float elapsedTime = 0f;
        while(elapsedTime < _waitTime)
        {
            elapsedTime += Time.deltaTime * _timeVariable;
            yield return null;
        }

        Enemy patrolEnemy = ChooseRandomEnemy();
        patrolEnemy.OnPatrolEnd += SendNextPatrol;
        Debug.Log("Sending Patrol");
        patrolEnemy.Patrol();
        
    }

    private void SendNextPatrol(Enemy enemy)
    {
        enemy.OnPatrolEnd -= SendNextPatrol;

        StartCoroutine(RandomPatrolEnemy());
    }
    private Enemy ChooseRandomEnemy()
    {
        return _enemyUnit[UnityEngine.Random.Range(0,_enemyUnit.Count)];
    }

    private void ChangeTimeVariable(float newValue)
    {
        _timeVariable = newValue;
    }
}
