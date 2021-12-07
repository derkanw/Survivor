using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemiesManager : MonoBehaviour
{
    public event Action<float> ChangedKilledCount;
    public event Action<float> SetPoints;
    public event Action PlayerWin;

    private event Action<Vector3> NotifiedEnemies;
    private event Action PlayerDied;
    private event Action<int> LevelUp;

    [SerializeField] private List<GameObject> enemies;
    [SerializeField] [Range(0f, 20f)] private float SpawnTime;

    [SerializeField] private Stat EnemiesCount;

    private float _currentCount;
    private float _groundWidth;
    private int _killedCount;
    private bool _playerExists;
    private int _index;

    public void SaveParams() => SaveSystem.Save<int>(Tokens.EnemyIndex, _index);

    public void MoveEnemiesTo(Vector3 position) => NotifiedEnemies?.Invoke(position);

    public void NotifyEnemies()
    {
        _playerExists = false;
        PlayerDied?.Invoke();
    }

    public void OnLevelUp(int level)
    {
        EnemiesCount.Modify(level);
        LevelUp?.Invoke(level);
        SaveSystem.Delete(Tokens.EnemyIndex);
    }

    private void Awake()
    {
        _groundWidth = 20f;
        _playerExists = true;
        EnemiesCount.Init();
        _index = SaveSystem.IsExists(Tokens.EnemyIndex) ? SaveSystem.Load<int>(Tokens.EnemyIndex) : UnityEngine.Random.Range(0, enemies.Count);
        StartCoroutine(InitEnemies(_index));
    }

    private void OnChangeKilledCount(BaseEnemy enemy, float points)
    {
        enemy.EnemyDied -= OnChangeKilledCount;
        NotifiedEnemies -= enemy.MoveTo;
        PlayerDied -= enemy.Stay;
        LevelUp -= enemy.OnLevelUp;

        ChangedKilledCount?.Invoke((float)++_killedCount / (int)EnemiesCount.Value); ;
        SetPoints?.Invoke(points);
        if (_killedCount == (int)EnemiesCount.Value)
            PlayerWin?.Invoke();
    }

    private IEnumerator InitEnemies(int number)
    {
        while (_playerExists && _currentCount < (int)EnemiesCount.Value)
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
