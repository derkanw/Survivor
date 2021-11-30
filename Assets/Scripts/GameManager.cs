using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private StatsUIManager Stats;
    [SerializeField] private EnemiesManager Enemies;
    [SerializeField] private InputSystem Input;
    [SerializeField] private PauseUIManager Pause;
    [SerializeField] private GameState State;

    [SerializeField] private CameraMovement MainCamera;
    [SerializeField] private GameObject LevelHUD;
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private GameObject Loot;

    private Player _playerParams;
    private WeaponsManager _weaponsManager;
    private SkillsManager _skillsManager;
    private LevelHUDManager _hud;
    private ButtonManager _buttonManager;
    private WeaponLoot _weaponLoot;

    private void Awake()
    {
        // TODO: load environment
        var levelHud = Instantiate(LevelHUD, Vector3.zero, Quaternion.identity);
        _hud = levelHud.GetComponent<LevelHUDManager>();

        Enemies.ChangedKilledCount += _hud.ChangeBulletBar;
        Enemies.SetPoints += State.OnSetPoints;
        Enemies.PlayerWin += State.LoadNextLevel;

        var player = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        _playerParams = player.GetComponent<Player>();
        _playerParams.ChangedHP += _hud.ChangeHealthBar;
        _playerParams.Moved += MainCamera.Move;
        _playerParams.Moved += Enemies.MoveEnemiesTo;
        _playerParams.Died += State.OnPlayerDied;
        _playerParams.Died += MainCamera.Stay;
        _playerParams.Died += Enemies.NotifyEnemies;

        _weaponsManager = player.GetComponent<WeaponsManager>();
        _weaponsManager.ChangedClipSize += _hud.OnChangedClipSize;
        _weaponsManager.ChangedBulletsCount += _hud.OnChangedBulletsCount;
        _weaponsManager.Reloading += _hud.ChangeReloadBar;
        _weaponsManager.GetWeaponsCount += Input.SetWeaponCount;
        _weaponsManager.GetWeaponsCount += _hud.ViewWeapons;

        _skillsManager = player.GetComponent<SkillsManager>();
        _skillsManager.GetSkillsCount += Input.SetSkillsCount;
        _skillsManager.ChangedSkillCount += _hud.ViewSkill;
        _skillsManager.ChangedSkillCount += Input.OnChangedSkillCount;
        _skillsManager.ChangedSkillReload += _hud.ChangeSkillReloadingBar;

        var lootUI = Instantiate(Loot, Vector3.zero, Quaternion.identity);
        _weaponLoot = lootUI.GetComponent<WeaponLoot>();
        _weaponsManager.GetArsenalSize += _weaponLoot.SetArsenalSize;
        _weaponLoot.LootSpawned += State.Notify;


        Input.CursorMoved += _playerParams.LookTo;
        Input.CursorMoved += _weaponsManager.LookTo;
        Input.ChangedPosition += _playerParams.MoveTo;
        Input.CursorClicked += _weaponsManager.SetShooting;
        Input.Reloading += _weaponsManager.SetReloading;
        Input.ChangeWeapon += _weaponsManager.SetArsenal;
        Input.ChangeWeapon += _hud.OnChangedWeapon;
        Input.ChangeSkill += _hud.OnChangedSkill;
        Input.ChangeSkill += _skillsManager.SetSkill;
        Input.UseSkill += _skillsManager.UseSkill;

        Pause.Resume += Input.OnResume;
        Stats.Resume += Input.OnResume;

        _buttonManager = levelHud.GetComponent<ButtonManager>();
        _buttonManager.Pause += Pause.OnPause;
        _buttonManager.Pause += Input.OnPause;
        _buttonManager.LooksStats += Stats.OnLooksStats;
        _buttonManager.LooksStats += Input.OnPause;

        Pause.SaveProgress += State.SaveParams;
        Pause.SaveProgress += Stats.SaveStats;
        Pause.SaveProgress += Enemies.SaveParams;

        State.PlayerLevelUp += _hud.OnPlayerLevelUp;
        State.ChangePoints += _hud.OnChangedPoints;

        State.ChangePoints += Stats.OnChangedPoints;
        Stats.GetPoints += _hud.OnChangedPoints;
        Stats.GetStats += _playerParams.OnLevelUp;
        State.LevelUp += Enemies.OnLevelUp;
        State.LevelUp += _hud.OnGameLevelUp;
        State.Disable += OnDisable;
    }

    private void Start() => State.InitDependencies(_weaponLoot, _weaponsManager, Stats);

    private void OnDisable()
    {
        Enemies.PlayerWin -= State.LoadNextLevel;
        Enemies.ChangedKilledCount -= _hud.ChangeBulletBar;
        Enemies.SetPoints -= State.OnSetPoints;

        _playerParams.ChangedHP -= _hud.ChangeHealthBar;
        _playerParams.Moved -= MainCamera.Move;
        _playerParams.Moved -= Enemies.MoveEnemiesTo;
        _playerParams.Died -= State.OnPlayerDied;
        _playerParams.Died -= MainCamera.Stay;
        _playerParams.Died -= Enemies.NotifyEnemies;

        _weaponsManager.ChangedClipSize -= _hud.OnChangedClipSize;
        _weaponsManager.ChangedBulletsCount -= _hud.OnChangedBulletsCount;
        _weaponsManager.Reloading -= _hud.ChangeReloadBar;
        _weaponsManager.GetWeaponsCount -= Input.SetWeaponCount;
        _skillsManager.ChangedSkillCount -= _hud.ViewSkill;
        _skillsManager.ChangedSkillCount -= Input.OnChangedSkillCount;
        _skillsManager.GetSkillsCount -= Input.SetSkillsCount;

        _weaponLoot.LootSpawned -= _weaponsManager.SetNewWeapon;
        _weaponsManager.GetArsenalSize -= _weaponLoot.SetArsenalSize;

        Input.CursorMoved -= _playerParams.LookTo;
        Input.CursorMoved -= _weaponsManager.LookTo;
        Input.ChangedPosition -= _playerParams.MoveTo;
        Input.CursorClicked -= _weaponsManager.SetShooting;
        Input.Reloading -= _weaponsManager.SetReloading;
        Input.ChangeWeapon -= _weaponsManager.SetArsenal;
        Input.ChangeWeapon -= _hud.OnChangedWeapon;
        Input.ChangeSkill -= _hud.OnChangedSkill;
        Input.ChangeSkill -= _skillsManager.SetSkill;
        Input.UseSkill -= _skillsManager.UseSkill;

        Pause.SaveProgress -= State.SaveParams;
        Pause.SaveProgress -= Stats.SaveStats;
        Pause.SaveProgress -= Enemies.SaveParams;
        Pause.Resume -= Input.OnResume;
        Stats.Resume -= Input.OnResume;

        _buttonManager.Pause -= Pause.OnPause;
        _buttonManager.Pause -= Input.OnPause;
        _buttonManager.LooksStats -= Stats.OnLooksStats;
        _buttonManager.LooksStats -= Input.OnPause;

        State.PlayerLevelUp -= _hud.OnPlayerLevelUp;
        State.ChangePoints -= _hud.OnChangedPoints;

        State.ChangePoints -= Stats.OnChangedPoints;
        Stats.GetPoints -= _hud.OnChangedPoints;
        Stats.GetStats -= _playerParams.OnLevelUp;

        State.LevelUp -= Enemies.OnLevelUp;
        State.LevelUp -= _hud.OnGameLevelUp;
    }
}
