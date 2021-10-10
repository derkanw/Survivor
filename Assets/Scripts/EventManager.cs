using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    [SerializeField] private GameObject LevelUI;
    [SerializeField] private EnemiesManager ManagerForEnemies;
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private InputSystem Input;
    [SerializeField] private CameraMovement camera;
    [SerializeField] private GameObject GameOverUI;

    private Player _playerParams;
    private BaseGun _gun;

    private void Start()
    {
        var ui = Instantiate(LevelUI, Vector3.zero, Quaternion.identity);
        var uiManager = ui.GetComponent<UIManager>();
        ManagerForEnemies.OnChangedKilledCount += uiManager.ChangeBulletBar;
        var player = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        _playerParams = player.GetComponent<Player>();
        _playerParams.OnChangedHP += uiManager.ChangeHealthBar;
        _playerParams.OnMoved += camera.ToPlayer;
        _playerParams.OnMoved += ManagerForEnemies.ToMoveEnemies;
        _playerParams.OnDied += PlayerDied;
        _playerParams.OnDied += camera.ToStay;
        _playerParams.OnDied += ManagerForEnemies.ToNotifyEnemies;

        _gun = GameObject.FindWithTag("Gun").GetComponent<BaseGun>();
        _gun.OnReload += uiManager.ChangeReloadBar;
        _gun.OnChangedBulletsCount += uiManager.ChangeBulletsCount;

        Input.OnMouseMoved += _playerParams.ToLook;
        Input.OnMouseMoved += _gun.ToLook;
        Input.OnChangedPosition += _playerParams.ToMove;
        Input.OnMouseClicked += _gun.ToMouseDown;
        Input.OnReloadingClicked += _gun.ToReloadingKeyDown;
    }

    public void PlayerDied()
    {
        Input.OnMouseMoved -= _playerParams.ToLook;
        Input.OnChangedPosition -= _playerParams.ToMove;

        Input.OnMouseMoved -= _gun.ToLook;
        Input.OnMouseClicked -= _gun.ToMouseDown;
        Input.OnReloadingClicked -= _gun.ToReloadingKeyDown;

        var ui = Instantiate(GameOverUI, Vector3.zero, Quaternion.identity);
    }
}
