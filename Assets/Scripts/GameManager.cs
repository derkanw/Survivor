using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    [SerializeField] private GameObject LevelHUD;
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private CameraMovement MainCamera;
    [SerializeField] private GameObject GameOverUI;

    private Player _playerParams;
    private BaseGun _gun;

    private float _points;
    private int _playerLevel;
    private int _gameLevel;

    private void Start()
    {
        PointsTarget.Init();

        var levelHud = Instantiate(LevelHUD, Vector3.zero, Quaternion.identity);
        var hud = levelHud.GetComponent<LevelHUDManager>();

        Enemies.OnChangedKilledCount += hud.ChangeBulletBar;
        Enemies.SetPoints += OnSetPoints;

        var player = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        _playerParams = player.GetComponent<Player>();
        _playerParams.OnChangedHP += hud.ChangeHealthBar;
        _playerParams.OnMoved += MainCamera.ToPlayer;
        _playerParams.OnMoved += Enemies.ToMoveEnemies;
        _playerParams.OnDied += PlayerDied;
        _playerParams.OnDied += MainCamera.ToStay;
        _playerParams.OnDied += Enemies.ToNotifyEnemies;

        _gun = GameObject.FindWithTag("Gun").GetComponent<BaseGun>();
        _gun.OnReload += hud.ChangeReloadBar;
        _gun.OnChangedBulletsCount += hud.ChangeBulletsCount;

        Input.OnMouseMoved += _playerParams.ToLook;
        Input.OnMouseMoved += _gun.ToLook;
        Input.OnChangedPosition += _playerParams.ToMove;
        Input.OnMouseClicked += _gun.ToMouseDown;
        Input.OnReloadingClicked += _gun.ToReloadingKeyDown;

        Pause.Resume += Input.OnResume;
        Stats.Resume += Input.OnResume;

        var buttonManager = levelHud.GetComponent<ButtonManager>();
        buttonManager.Pause += Pause.OnPause;
        buttonManager.Pause += Input.OnPause;
        buttonManager.LooksStats += Stats.OnLooksStats;
        buttonManager.LooksStats += Input.OnPause;

        PlayerLevelUp += hud.OnLevelUp;
        ChangePoints += hud.OnChangedPoints;

        ChangePoints += Stats.OnChangedPoints;
        Stats.GetPoints += hud.OnChangedPoints;
        Stats.GetStats += _playerParams.OnLevelUp;
        LevelUp += Enemies.OnLevelUp;
    }

    private void PlayerDied()
    {
        Input.OnMouseMoved -= _playerParams.ToLook;
        Input.OnChangedPosition -= _playerParams.ToMove;
        Stats.GetStats -= _playerParams.OnLevelUp;

        Input.OnMouseMoved -= _gun.ToLook;
        Input.OnMouseClicked -= _gun.ToMouseDown;
        Input.OnReloadingClicked -= _gun.ToReloadingKeyDown;

        var ui = Instantiate(GameOverUI, Vector3.zero, Quaternion.identity);
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
}
