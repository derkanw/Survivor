using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private uint LevelsCount;
    [SerializeField] private Stat PointsTarget;

    [SerializeField] private StatsUIManager Stats;
    [SerializeField] private EnemiesManager Enemies;
    [SerializeField] private InputSystem Input;
    [SerializeField] private PauseUIManager Pause;
    [SerializeField] private MainMenuManager Menu;

    [SerializeField] private CameraMovement MainCamera;
    [SerializeField] private GameObject LevelHUD;
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private GameObject GameOverUI;

    private event Action<int> PlayerLevelUp;
    private event Action<int> LevelUp;
    private event Action<int> ChangePoints;

    private Player _playerParams;
    private WeaponsManager _weaponsManager;
    private float _points;
    private int _playerLevel;
    private int _gameLevel;

    private void Awake()
    {
        Menu.SetActive();
        PointsTarget.Init();

        var levelHud = Instantiate(LevelHUD, Vector3.zero, Quaternion.identity);
        var hud = levelHud.GetComponent<LevelHUDManager>();

        Enemies.ChangedKilledCount += hud.ChangeBulletBar;
        Enemies.SetPoints += OnSetPoints;
        Enemies.PlayerWin += LoadNextLevel;

        var player = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        _playerParams = player.GetComponent<Player>();
        _playerParams.ChangedHP += hud.ChangeHealthBar;
        _playerParams.Moved += MainCamera.Move;
        _playerParams.Moved += Enemies.MoveEnemiesTo;
        _playerParams.Died += OnPlayerDied;
        _playerParams.Died += MainCamera.Stay;
        _playerParams.Died += Enemies.NotifyEnemies;
        _playerParams.Died += Menu.OnFailed;

        _weaponsManager = player.GetComponent<WeaponsManager>();
        _weaponsManager.ChangedClipSize += hud.OnChangedClipSize;
        _weaponsManager.ChangedBulletsCount += hud.OnChangedBulletsCount;
        _weaponsManager.Reloading += hud.ChangeReloadBar;
        _weaponsManager.GetWeaponsCount += Input.SetWeaponCount;

        Input.CursorMoved += _playerParams.LookTo;
        Input.CursorMoved += _weaponsManager.LookTo;
        Input.ChangedPosition += _playerParams.MoveTo;
        Input.CursorClicked += _weaponsManager.SetShooting;
        Input.ReloadingClicked += _weaponsManager.SetReloading;
        Input.ChangeWeapon += _weaponsManager.SetArsenal;
        Input.ChangeWeapon += hud.OnChangedWeapon;

        Pause.Resume += Input.OnResume;
        Stats.Resume += Input.OnResume;

        var buttonManager = levelHud.GetComponent<ButtonManager>();
        buttonManager.Pause += Pause.OnPause;
        buttonManager.Pause += Input.OnPause;
        buttonManager.LooksStats += Stats.OnLooksStats;
        buttonManager.LooksStats += Input.OnPause;

        Pause.SaveProgress += SaveParams;
        Pause.SaveProgress += Stats.SaveStats;

        PlayerLevelUp += hud.OnPlayerLevelUp;
        ChangePoints += hud.OnChangedPoints;

        ChangePoints += Stats.OnChangedPoints;
        Stats.GetPoints += hud.OnChangedPoints;
        Stats.GetStats += _playerParams.OnLevelUp;
        LevelUp += Enemies.OnLevelUp;
        LevelUp += hud.OnGameLevelUp;
    }

    private void Start()
    {
        _playerLevel = PlayerPrefs.GetInt("PlayerLevel", 0);
        _gameLevel = PlayerPrefs.GetInt("GameLevel", 0);
        _points = PlayerPrefs.GetFloat("PointsForNewLevel", 0);
        PlayerLevelUp?.Invoke(_playerLevel);
        PointsTarget.Modify(_playerLevel);
        LevelUp?.Invoke(_gameLevel);
    }

    private void OnPlayerDied()
    {
        Input.CursorMoved -= _playerParams.LookTo;
        Input.ChangedPosition -= _playerParams.MoveTo;
        Stats.GetStats -= _playerParams.OnLevelUp;

        Input.CursorMoved -= _weaponsManager.LookTo;
        Input.CursorClicked -= _weaponsManager.SetShooting;
        Input.ReloadingClicked -= _weaponsManager.SetReloading;

        var ui = Instantiate(GameOverUI, Vector3.zero, Quaternion.identity);
        PlayerPrefs.DeleteAll();
        File.Delete(SaveSystem.path);
    }

    private void OnSetPoints(float points)
    {
        _points += points;
        if (_points >= PointsTarget.Value)
        {
            ++_playerLevel;
            ChangePoints?.Invoke(_playerLevel);
            PlayerLevelUp?.Invoke(_playerLevel);
            PointsTarget.Modify(_playerLevel);
            _points = 0;
        }
    }

    private void SaveParams()
    {
        PlayerPrefs.SetInt("PlayerLevel", _playerLevel);
        PlayerPrefs.SetInt("GameLevel", _gameLevel);
        PlayerPrefs.SetFloat("PointsForNewLevel", _points);
    }

    private void LoadNextLevel()
    {
        ++_gameLevel;
        SaveParams();
        Stats.SaveStats();
        SceneManager.LoadScene(_gameLevel >= LevelsCount ? ("Scenes/MainMenu") : (SceneManager.GetActiveScene().name));
    }
}
