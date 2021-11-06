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
    private LevelHUDManager _hud;
    private ButtonManager _buttonManager;
    private float _points;
    private int _playerLevel;
    private int _gameLevel;

    private void Awake()
    {
        Menu.SetActive();
        PointsTarget.Init();

        var levelHud = Instantiate(LevelHUD, Vector3.zero, Quaternion.identity);
        _hud = levelHud.GetComponent<LevelHUDManager>();

        Enemies.ChangedKilledCount += _hud.ChangeBulletBar;
        Enemies.SetPoints += OnSetPoints;
        Enemies.PlayerWin += LoadNextLevel;

        var player = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        _playerParams = player.GetComponent<Player>();
        _playerParams.ChangedHP += _hud.ChangeHealthBar;
        _playerParams.Moved += MainCamera.Move;
        _playerParams.Moved += Enemies.MoveEnemiesTo;
        _playerParams.Died += OnPlayerDied;
        _playerParams.Died += MainCamera.Stay;
        _playerParams.Died += Enemies.NotifyEnemies;
        _playerParams.Died += Menu.OnFailed;

        _weaponsManager = player.GetComponent<WeaponsManager>();
        _weaponsManager.ChangedClipSize += _hud.OnChangedClipSize;
        _weaponsManager.ChangedBulletsCount += _hud.OnChangedBulletsCount;
        _weaponsManager.Reloading += _hud.ChangeReloadBar;
        _weaponsManager.GetWeaponsCount += Input.SetWeaponCount;

        Input.CursorMoved += _playerParams.LookTo;
        Input.CursorMoved += _weaponsManager.LookTo;
        Input.ChangedPosition += _playerParams.MoveTo;
        Input.CursorClicked += _weaponsManager.SetShooting;
        Input.Reloading += _weaponsManager.SetReloading;
        Input.ChangeWeapon += _weaponsManager.SetWeapon;
        Input.ChangeWeapon += _hud.OnChangedWeapon;

        Pause.Resume += Input.OnResume;
        Stats.Resume += Input.OnResume;

        _buttonManager = levelHud.GetComponent<ButtonManager>();
        _buttonManager.Pause += Pause.OnPause;
        _buttonManager.Pause += Input.OnPause;
        _buttonManager.Restart += OnDisable;
        _buttonManager.LooksStats += Stats.OnLooksStats;
        _buttonManager.LooksStats += Input.OnPause;

        Pause.SaveProgress += SaveParams;
        Pause.SaveProgress += Stats.SaveStats;

        PlayerLevelUp += _hud.OnPlayerLevelUp;
        ChangePoints += _hud.OnChangedPoints;

        ChangePoints += Stats.OnChangedPoints;
        Stats.GetPoints += _hud.OnChangedPoints;
        Stats.GetStats += _playerParams.OnLevelUp;
        LevelUp += Enemies.OnLevelUp;
        LevelUp += _hud.OnGameLevelUp;
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

    private void OnDisable()
    {
        Enemies.PlayerWin -= LoadNextLevel;
        Enemies.ChangedKilledCount -= _hud.ChangeBulletBar;
        Enemies.SetPoints -= OnSetPoints;

        _playerParams.ChangedHP -= _hud.ChangeHealthBar;
        _playerParams.Moved -= MainCamera.Move;
        _playerParams.Moved -= Enemies.MoveEnemiesTo;
        _playerParams.Died -= OnPlayerDied;
        _playerParams.Died -= MainCamera.Stay;
        _playerParams.Died -= Enemies.NotifyEnemies;
        _playerParams.Died -= Menu.OnFailed;

        _weaponsManager.ChangedClipSize -= _hud.OnChangedClipSize;
        _weaponsManager.ChangedBulletsCount -= _hud.OnChangedBulletsCount;
        _weaponsManager.Reloading -= _hud.ChangeReloadBar;
        _weaponsManager.GetWeaponsCount -= Input.SetWeaponCount;

        Input.CursorMoved -= _playerParams.LookTo;
        Input.CursorMoved -= _weaponsManager.LookTo;
        Input.ChangedPosition -= _playerParams.MoveTo;
        Input.CursorClicked -= _weaponsManager.SetShooting;
        Input.Reloading -= _weaponsManager.SetReloading;
        Input.ChangeWeapon -= _weaponsManager.SetWeapon;
        Input.ChangeWeapon -= _hud.OnChangedWeapon;

        Pause.SaveProgress -= SaveParams;
        Pause.SaveProgress -= Stats.SaveStats;
        Pause.Resume -= Input.OnResume;
        Stats.Resume -= Input.OnResume;

        _buttonManager.Pause -= Pause.OnPause;
        _buttonManager.Pause -= Input.OnPause;
        _buttonManager.Restart -= OnDisable;
        _buttonManager.LooksStats -= Stats.OnLooksStats;
        _buttonManager.LooksStats -= Input.OnPause;

        PlayerLevelUp -= _hud.OnPlayerLevelUp;
        ChangePoints -= _hud.OnChangedPoints;

        ChangePoints -= Stats.OnChangedPoints;
        Stats.GetPoints -= _hud.OnChangedPoints;
        Stats.GetStats -= _playerParams.OnLevelUp;

        LevelUp -= Enemies.OnLevelUp;
        LevelUp -= _hud.OnGameLevelUp;
    }

    private void OnPlayerDied()
    {
        OnDisable();
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
        OnDisable();
    }

    private void LoadNextLevel()
    {
        ++_gameLevel;
        SaveParams();
        Stats.SaveStats();
        SceneManager.LoadScene(_gameLevel >= LevelsCount ? ("Scenes/MainMenu") : (SceneManager.GetActiveScene().name));
    }
}
