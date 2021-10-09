using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemiesManager : MonoBehaviour
{
    [HideInInspector] public float KilledCount; // change to event system
    [SerializeField] private Image Bar;

    [SerializeField] private List<GameObject> enemies;
    [SerializeField] [Range(0f, 20f)] private float SpawnTime;
    [SerializeField] [Range(0f, 50f)] private float EnemiesCount;

    private float _currentCount;
    private float _groundWidth;

    private void Start()
    {
        _groundWidth = 9f;
        StartCoroutine(InitEnemies(0));
    }

    private void Update()
    {
        Bar.fillAmount = KilledCount / EnemiesCount;
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
}
