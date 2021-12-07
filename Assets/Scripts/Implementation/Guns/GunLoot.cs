using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GunLoot : MonoBehaviour, IGunLoot
{
    public event Action LootSpawned;
    [SerializeField] [Range(0f, 1f)] private float Chance;
    [SerializeField] private Button AcceptButton;
    [SerializeField] private Image Icon;
    [SerializeField] private List<Sprite> WeaponIcons;

    private string _sceneName;
    private int _index;
    private int _count;

    public void SetArsenalSize(int size) => _count = size;

    public void SpawnLoot(string sceneName)
    {
        if (UnityEngine.Random.Range(0f, 1f) >= (1 - Chance) && _index <= _count - 1)
        {
            gameObject.SetActive(true);
            ButtonModel.SetUpButton(AcceptButton, LoadNextLevel);
            Icon.sprite = WeaponIcons[_index - 1];
            _sceneName = sceneName;
        }
        else
            SceneManager.LoadScene(sceneName);
    }

    private void LoadNextLevel()
    {
        SaveSystem.Save<int>(Tokens.LootIndex, ++_index);
        LootSpawned?.Invoke();
        SceneManager.LoadScene(_sceneName);
    }

    private void Awake()
    {
        gameObject.SetActive(false);
        _index = SaveSystem.IsExists(Tokens.LootIndex) ? SaveSystem.Load<int>(Tokens.LootIndex) : 1;
    }
}
