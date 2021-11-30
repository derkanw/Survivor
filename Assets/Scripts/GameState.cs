using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    public event Action<int> PlayerLevelUp;
    public event Action<int> LevelUp;
    public event Action<int> ChangePoints;
    public event Action Disable;

    [SerializeField] private uint LevelsCount;
    [SerializeField] private Stat PointsTarget;
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private MainMenuManager Menu;

    private float _points;
    private int _playerLevel;
    private int _gameLevel;

    private WeaponsManager _weaponsManager;
    private WeaponLoot _weaponLoot;
    private StatsUIManager _stats;

    public void InitDependencies(WeaponLoot loot, WeaponsManager manager, StatsUIManager stats)
    {
        _weaponLoot = loot;
        _weaponsManager = manager;
        _stats = stats;
    }

    public void Notify() => _weaponsManager.SetNewWeapon();

    public void OnPlayerDied()
    {
        Disable?.Invoke();
        Menu.SetActive(false);
        Instantiate(GameOverUI, Vector3.zero, Quaternion.identity);
        SaveSystem.DeleteAll();
    }

    public void OnSetPoints(float points)
    {
        _points += points;
        if (_points >= PointsTarget.Value)
        {
            AudioManager.PlaySound(SoundNames.PlayerLevel);
            ++_playerLevel;
            ChangePoints?.Invoke(_playerLevel);
            PlayerLevelUp?.Invoke(_playerLevel);
            PointsTarget.Modify(_playerLevel);
            _points = 0;
        }
    }

    public void SaveParams()
    {
        SaveSystem.Save<int>(Tokens.PlayerLevel, _playerLevel);
        SaveSystem.Save<int>(Tokens.GameLevel, _gameLevel);
        SaveSystem.Save<float>(Tokens.Points, _points);
        Disable?.Invoke();
    }

    public void LoadNextLevel()
    {
        ++_gameLevel;
        SaveParams();
        _stats.SaveStats();
        _weaponLoot.SpawnLoot(_gameLevel >= LevelsCount ? Tokens.MainMenu : SceneManager.GetActiveScene().name);
    }

    private void Awake()
    {
        PointsTarget.Init();
        Menu.SetActive(true);
    }

    private void Start()
    {
        _playerLevel = SaveSystem.Load<int>(Tokens.PlayerLevel);
        _gameLevel = SaveSystem.Load<int>(Tokens.GameLevel);
        _points = SaveSystem.Load<float>(Tokens.Points);
        PlayerLevelUp?.Invoke(_playerLevel);
        PointsTarget.Modify(_playerLevel);
        LevelUp?.Invoke(_gameLevel);
    }
}
