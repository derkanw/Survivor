using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class GameManager : MonoBehaviour
{
    private event Action<int> PlayerLevelUp;
    private event Action<int> LevelUp;
    private event Action<int> ChangePoints;

    [SerializeField] private Stat PointsTarget;

    [SerializeField] private StatsUIManager Stats;
    [SerializeField] private EnemiesManager Enemies;
    [SerializeField] private InputSystem Input;
    [SerializeField] private PauseUIManager Pause;
    [SerializeField] private MainMenuManager Menu;

    [SerializeField] private GameObject LevelHUD;
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private CameraMovement MainCamera;
    [SerializeField] private GameObject GameOverUI;

    private Player _playerParams;
    private BaseGun _gun;

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

        var player = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        _playerParams = player.GetComponent<Player>();
        _playerParams.ChangedHP += hud.ChangeHealthBar;
        _playerParams.Moved += MainCamera.Move;
        _playerParams.Moved += Enemies.MoveEnemiesTo;
        _playerParams.Died += OnPlayerDied;
        _playerParams.Died += MainCamera.Stay;
        _playerParams.Died += Enemies.NotifyEnemies;
        _playerParams.Died += Menu.OnFailed;

        _gun = GameObject.FindWithTag("Gun").GetComponent<BaseGun>();
        _gun.ChangedClipSize += hud.OnChangedClipSize;
        _gun.ChangedBulletsCount += hud.OnChangedBulletsCount;
        _gun.Reloading += hud.ChangeReloadBar;

        // need to change
        Input.CursorMoved += _playerParams.LookTo;
        Input.CursorMoved += _gun.LookTo;
        Input.ChangedPosition += _playerParams.MoveTo;
        Input.MouseClicked += _gun.OnMouseDown;
        Input.Reloading += _gun.OnReloadingKeyDown;

        Pause.Resume += Input.OnResume;
        Stats.Resume += Input.OnResume;

        var buttonManager = levelHud.GetComponent<ButtonManager>();
        buttonManager.Pause += Pause.OnPause;
        buttonManager.Pause += Input.OnPause;
        buttonManager.LooksStats += Stats.OnLooksStats;
        buttonManager.LooksStats += Input.OnPause;

        Pause.SaveProgress += SaveParams;
        Pause.SaveProgress += Stats.SaveStats;

        PlayerLevelUp += hud.OnLevelUp;
        ChangePoints += hud.OnChangedPoints;

        ChangePoints += Stats.OnChangedPoints;
        Stats.GetPoints += hud.OnChangedPoints;
        Stats.GetStats += _playerParams.OnLevelUp;
        LevelUp += Enemies.OnLevelUp;

        _playerLevel = PlayerPrefs.GetInt("PlayerLevel", 0);
        _gameLevel = PlayerPrefs.GetInt("GameLevel", 0);
        _points = PlayerPrefs.GetFloat("PointsForNewLevel", 0);
        PlayerLevelUp?.Invoke(_playerLevel);
        PointsTarget.Modify(_playerLevel);
    }

    private void OnPlayerDied()
    {
        Input.CursorMoved -= _playerParams.LookTo;
        Input.ChangedPosition -= _playerParams.MoveTo;
        Stats.GetStats -= _playerParams.OnLevelUp;

        Input.CursorMoved -= _gun.LookTo;
        Input.MouseClicked -= _gun.OnMouseDown;
        Input.Reloading -= _gun.OnReloadingKeyDown;

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
            ChangePoints?.Invoke(_playerLevel * 2);
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
}
