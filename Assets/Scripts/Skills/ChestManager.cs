using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    [SerializeField] private Stat BoxesCount;
    [SerializeField] [Range(0f, 120f)] private float SpawnTime;
    [SerializeField] private GameObject BoxPrefab;

    private float _currentCount;
    private float _groundWidth;
    private bool _playerExists;

    public void StopSpawning() => _playerExists = false;

    public void OnLevelUp(int level)
    {
        BoxesCount.Modify(level);
        SaveSystem.Save<int>(Tokens.Boxes, (int)BoxesCount.Value);
    }

    private void Awake()
    {
        _groundWidth = 22f;
        _playerExists = true;
        BoxesCount.Init();
        StartCoroutine(InitBoxes());
    }

    private void GetDestroyedBoxes()
    {
        --_currentCount;
        StartCoroutine(InitBoxes());
    }

    private IEnumerator InitBoxes()
    {
        while (_playerExists && _currentCount < BoxesCount.Value)
        {
            float time = Random.Range(0f, SpawnTime);
            Vector3 position = new Vector3(Random.Range(-_groundWidth, _groundWidth), 0, Random.Range(-_groundWidth, _groundWidth));
            GameObject box = Instantiate(BoxPrefab, position, Quaternion.identity);
            ++_currentCount;
            box.GetComponent<Chest>().BoxDestroyed += GetDestroyedBoxes;
            yield return new WaitForSeconds(time);
        }
    }
}
