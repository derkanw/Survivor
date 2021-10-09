using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] private GameObject LevelUI;
    [SerializeField] private EnemiesManager ManagerForEnemies;
    [SerializeField] private GameObject Player;
    [SerializeField] private InputSystem Input;
    [SerializeField] private CameraMovement camera;

    private void Awake()
    {
        var _ui = Instantiate(LevelUI, Vector3.zero, Quaternion.identity);
        var _uiManager = _ui.GetComponent<UIManager>();
        ManagerForEnemies.OnChangedKilledCount += _uiManager.ChangeBulletBar;
        var playerParams = Player.GetComponent<Player>();
        playerParams.OnChangedHP += _uiManager.ChangeHealthBar;
        playerParams.OnMoved += camera.ToPlayer;
        playerParams.OnMoved += ManagerForEnemies.ToMoveEnemies;

        var _gun = GameObject.FindWithTag("Gun").GetComponent<BaseGun>();
        _gun.OnReload += _uiManager.ChangeReloadBar;
        _gun.OnChangedBulletsCount += _uiManager.ChangeBulletsCount;

        Input.OnMouseMoved += playerParams.ToLook;
        Input.OnMouseMoved += _gun.ToLook;
        Input.OnMouseMoved += camera.ToMouse;
        Input.OnChangedPosition += playerParams.ToMove;
        Input.OnMouseClicked += _gun.ToMouseDown;
        Input.OnReloadingClicked += _gun.ToReloadingKeyDown;
    }
}
