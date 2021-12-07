using UnityEngine;

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
    [SerializeField] private GameObject Environment;

    private Player _playerParams;
    private IGunService _gunService;
    private SkillsManager _skillsManager;
    private LevelHUDManager _hud;
    private ButtonManager _buttonManager;
    private IGunLoot _gunLoot;

    private void Awake()
    {
        Instantiate(Environment);
        var levelHud = Instantiate(LevelHUD, Vector3.zero, Quaternion.identity);
        _hud = levelHud.GetComponent<LevelHUDManager>();
        _buttonManager = levelHud.GetComponent<ButtonManager>();

        var lootUI = Instantiate(Loot, Vector3.zero, Quaternion.identity);
        _gunLoot = lootUI.GetComponent<IGunLoot>();

        var player = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        _playerParams = player.GetComponent<Player>();
        _gunService = player.GetComponent<IGunService>();
        _skillsManager = player.GetComponent<SkillsManager>();

        _playerParams.ChangedHP += _hud.ChangeHealthBar;
        _playerParams.Moved += MainCamera.Move;
        _playerParams.Moved += Enemies.MoveEnemiesTo;
        _playerParams.Died += MainCamera.Stay;
        _playerParams.Died += Enemies.NotifyEnemies;
        _playerParams.Died += State.OnPlayerDied;
        _playerParams.Died += _buttonManager.DisableButtons;
        _playerParams.Died += Input.DisableInput;

        _gunService.ChangedClipSize += _hud.OnChangedClipSize;
        _gunService.ChangedBulletCount += _hud.OnChangedBulletsCount;
        _gunService.Reloading += _hud.ChangeReloadBar;
        _gunService.GetGunCount += _hud.ViewWeapons;
        _gunService.GetGunCount += Input.SetWeaponCount;
        _gunService.GetArsenalSize += _gunLoot.SetArsenalSize;

        _gunLoot.LootSpawned += State.Notify;

        _skillsManager.GetSkillsCount += Input.SetSkillsCount;
        _skillsManager.ChangedSkillCount += Input.OnChangedSkillCount;
        _skillsManager.ChangedSkillCount += _hud.ViewSkill;
        _skillsManager.ChangedSkillReload += _hud.ChangeSkillReloadingBar;

        Enemies.ChangedKilledCount += _hud.ChangeBulletBar;
        Enemies.SetPoints += State.OnSetPoints;
        Enemies.PlayerWin += State.LoadNextLevel;
        Enemies.PlayerWin += Input.DisableInput;

        _buttonManager.Pause += Pause.OnPause;
        _buttonManager.Pause += Input.DisableInput;
        _buttonManager.LooksStats += Input.DisableInput;
        _buttonManager.LooksStats += Stats.OnLooksStats;

        Pause.Resume += Input.ActivateInput;
        Pause.SaveProgress += State.SaveParams;
        Pause.SaveProgress += Stats.SaveStats;
        Pause.SaveProgress += Enemies.SaveParams;
        Pause.SaveProgress += _playerParams.SaveParams;
        Pause.SaveProgress += _skillsManager.SaveParams;

        Stats.Resume += Input.ActivateInput;
        Stats.GetPoints += _hud.OnChangedPoints;
        Stats.GetStats += _playerParams.OnLevelUp;

        Input.ChangedPosition += _playerParams.MoveTo;
        Input.CursorMoved += _playerParams.LookTo;
        Input.CursorMoved += _gunService.LookTo;
        Input.CursorClicked += _gunService.SetShooting;
        Input.Reloading += _gunService.SetReloading;
        Input.ChangeWeapon += _gunService.SetArsenal;
        Input.ChangeWeapon += _hud.OnChangedWeapon;
        Input.ChangeSkill += _hud.OnChangedSkill;
        Input.ChangeSkill += _skillsManager.SetSkill;
        Input.UseSkill += _skillsManager.UseSkill;

        State.LevelUp += Enemies.OnLevelUp;
        State.LevelUp += _hud.OnGameLevelUp;
        State.PlayerLevelUp += _hud.OnPlayerLevelUp;
        State.ChangePoints += _hud.OnChangedPoints;
        State.ChangePoints += Stats.OnChangedPoints;
        State.Disable += OnDisable;
    }

    private void Start() => State.InitDependencies(_gunLoot, _gunService, Stats, _playerParams, _skillsManager);

    private void OnDisable()
    {
        Input.CursorMoved -= _playerParams.LookTo;
        Input.ChangedPosition -= _playerParams.MoveTo;
        Stats.GetStats -= _playerParams.OnLevelUp;
        Pause.SaveProgress -= _playerParams.SaveParams;

        _gunLoot.LootSpawned -= _gunService.SetNewGun;
        Input.CursorMoved -= _gunService.LookTo;
        Input.CursorClicked -= _gunService.SetShooting;
        Input.Reloading -= _gunService.SetReloading;
        Input.ChangeWeapon -= _gunService.SetArsenal;

        Input.ChangeSkill -= _skillsManager.SetSkill;
        Input.UseSkill -= _skillsManager.UseSkill;
        Pause.SaveProgress -= _skillsManager.SaveParams;
    }
}
