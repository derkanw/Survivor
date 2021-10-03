using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public List<GameObject> enemies;
    [Range(0f, 50f)]
    public float SpawnTime, EnemiesCount;
    private float _currentCount, _groundWidth;

    public void Start()
    {
        StartCoroutine(InitEnemies(0));

        _currentCount = 0;
        _groundWidth = 9f;
    }

    private IEnumerator InitEnemies(int number)
    {
        while (_currentCount < EnemiesCount)
        {
            float time = Random.Range(0f, SpawnTime);
            Vector3 position = new Vector3(Random.Range(-_groundWidth, _groundWidth), 0, Random.Range(-_groundWidth, _groundWidth));
            GameObject enemy = Instantiate(enemies[number], position, Quaternion.Euler(0f, Random.Range(-180f, 180f), 0f));
            ++_currentCount;
            yield return new WaitForSeconds(time);
        }
    }

    public void Update()
    {

    }
}
