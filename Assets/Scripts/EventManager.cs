using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    [SerializeField] private GameObject LevelUI;
    [SerializeField] private EnemiesManager ManagerForEnemies;
    [SerializeField] private GameObject Player;
    [SerializeField] private InputSystem Input;
    [SerializeField] private CameraMovement camera;
    [SerializeField] private GameObject GameOverUI;

    private void Start()
    {
        var _ui = Instantiate(LevelUI, Vector3.zero, Quaternion.identity);
        var _uiManager = _ui.GetComponent<UIManager>();
        ManagerForEnemies.OnChangedKilledCount += _uiManager.ChangeBulletBar;
        var playerParams = Player.GetComponent<Player>();
        playerParams.OnChangedHP += _uiManager.ChangeHealthBar;
        playerParams.OnMoved += camera.ToPlayer;
        playerParams.OnMoved += ManagerForEnemies.ToMoveEnemies;
        playerParams.OnDied += PlayerDied;
        playerParams.OnDied += camera.ToStay;
        playerParams.OnDied += ManagerForEnemies.ToNotifyEnemies;

        var _gun = GameObject.FindWithTag("Gun").GetComponent<BaseGun>();
        _gun.OnReload += _uiManager.ChangeReloadBar;
        _gun.OnChangedBulletsCount += _uiManager.ChangeBulletsCount;

        Input.OnMouseMoved += playerParams.ToLook;
        Input.OnMouseMoved += _gun.ToLook;
        Input.OnChangedPosition += playerParams.ToMove;
        Input.OnMouseClicked += _gun.ToMouseDown;
        Input.OnReloadingClicked += _gun.ToReloadingKeyDown;
    }

    public void PlayerDied()
    {
        var playerParams = Player.GetComponent<Player>();
        Input.OnMouseMoved -= playerParams.ToLook;
        Input.OnChangedPosition -= playerParams.ToMove;

        var _gun = GameObject.FindWithTag("Gun").GetComponent<BaseGun>();
        Input.OnMouseMoved -= _gun.ToLook;
        Input.OnMouseClicked -= _gun.ToMouseDown;
        Input.OnReloadingClicked -= _gun.ToReloadingKeyDown;

        var _ui = Instantiate(GameOverUI, Vector3.zero, Quaternion.identity);
    }
}
