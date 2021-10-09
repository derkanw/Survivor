using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemiesManager : MonoBehaviour
{
    public event Action<float> OnChangedKilledCount;
    public event Action<Vector3> OnNotifiedEnemies;

    [SerializeField] private List<GameObject> enemies;
    [SerializeField] [Range(0f, 20f)] private float SpawnTime;
    [SerializeField] [Range(0f, 50f)] private float EnemiesCount;

    private float _currentCount;
    private float _groundWidth;
    private int _killedCount;

    private void Start()
    {
        _groundWidth = 9f;
        StartCoroutine(InitEnemies(0));
    }

    private void ChangeKilledCount()
    {
        OnChangedKilledCount?.Invoke(++_killedCount / EnemiesCount);
    }

    private IEnumerator InitEnemies(int number)
    {
        while (_currentCount < EnemiesCount)
        {
            float time = UnityEngine.Random.Range(0f, SpawnTime);
            Vector3 position = new Vector3(UnityEngine.Random.Range(-_groundWidth, _groundWidth), 0, UnityEngine.Random.Range(-_groundWidth, _groundWidth));
            GameObject enemy = Instantiate(enemies[number], position, Quaternion.Euler(0f, UnityEngine.Random.Range(-180f, 180f), 0f));
            ++_currentCount;

            BaseEnemy enemyParams = enemy.GetComponent<BaseEnemy>();
            enemyParams.OnEnemyDied += ChangeKilledCount;
            OnNotifiedEnemies += enemyParams.ToMove;

            yield return new WaitForSeconds(time);
        }
    }

    public void ToMoveEnemies(Vector3 position) => OnNotifiedEnemies?.Invoke(position);
}
