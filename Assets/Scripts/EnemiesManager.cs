using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemiesManager : MonoBehaviour
{
    public event Action<float> ChangedKilledCount;
    public event Action<float> SetPoints;

    private event Action<Vector3> NotifiedEnemies;
    private event Action PlayerDied;
    private event Action<int> LevelUp;

    [SerializeField] private List<GameObject> enemies;
    [SerializeField] [Range(0f, 20f)] private float SpawnTime;
    [SerializeField] [Range(0f, 50f)] private float EnemiesCount;

    private float _currentCount;
    private float _groundWidth;
    private int _killedCount;

    public void MoveEnemiesTo(Vector3 position) => NotifiedEnemies?.Invoke(position);

    public void NotifyEnemies()
    {
        EnemiesCount = _currentCount;
        PlayerDied?.Invoke();
    }

    public void OnLevelUp(int level) => LevelUp?.Invoke(level);

    private void Start()
    {
        _groundWidth = 9f;
        StartCoroutine(InitEnemies(0));
    }

    private void OnChangeKilledCount(BaseEnemy enemy, float points)
    {
        NotifiedEnemies -= enemy.MoveTo;
        PlayerDied -= enemy.Stay;
        LevelUp -= enemy.OnLevelUp;

        ChangedKilledCount?.Invoke(++_killedCount / EnemiesCount);
        SetPoints?.Invoke(points);
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
            enemyParams.EnemyDied += OnChangeKilledCount;
            PlayerDied += enemyParams.Stay;
            NotifiedEnemies += enemyParams.MoveTo;
            LevelUp += enemyParams.OnLevelUp;

            yield return new WaitForSeconds(time);
        }
    }
}
