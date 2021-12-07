using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private StatsUIManager Stats;
    [SerializeField] private EnemiesManager Enemies;
    [SerializeField] private InputSystem Input;
    [SerializeField] private PauseUIManager Pause;

    [SerializeField] private CameraMovement MainCamera;
    [SerializeField] private GameObject LevelHUD;
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private GameObject Loot;
    [SerializeField] private GameObject Environment;

    private IGameState _state;

    private Player _playerParams;
    private IGunService _gunService;
    private SkillsManager _skillsManager;
    private ILevelView _hud;
    private ButtonManager _buttonManager;
    private IGunLoot _gunLoot;

    private void Awake()
    {
        _state = gameObject.GetComponent<IGameState>();

        Instantiate(Environment);
        var levelHud = Instantiate(LevelHUD, Vector3.zero, Quaternion.identity);
        _hud = levelHud.GetComponent<ILevelView>();
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
        _playerParams.Died += _state.OnPlayerDied;
        _playerParams.Died += _buttonManager.DisableButtons;
        _playerParams.Died += Input.DisableInput;

        _gunService.ChangedClipSize += _hud.OnChangedClipSize;
        _gunService.ChangedBulletCount += _hud.OnChangedBulletCount;
        _gunService.Reloading += _hud.ChangeReloadBar;
        _gunService.GetGunCount += _hud.ViewGuns;
        _gunService.GetGunCount += Input.SetGunCount;
        _gunService.GetArsenalSize += _gunLoot.SetArsenalSize;

        _gunLoot.LootSpawned += _state.Notify;

        _skillsManager.GetSkillsCount += Input.SetSkillCount;
        _skillsManager.ChangedSkillCount += Input.OnChangedSkillCount;
        _skillsManager.ChangedSkillCount += _hud.ViewSkill;
        _skillsManager.ChangedSkillReload += _hud.ChangeSkillReloadingBar;

        Enemies.ChangedKilledCount += _hud.ChangeBulletBar;
        Enemies.SetPoints += _state.OnSetPoints;
        Enemies.PlayerWin += _state.LoadNextLevel;
        Enemies.PlayerWin += Input.DisableInput;

        _buttonManager.Pause += Pause.OnPause;
        _buttonManager.Pause += Input.DisableInput;
        _buttonManager.LooksStats += Input.DisableInput;
        _buttonManager.LooksStats += Stats.OnLooksStats;

        Pause.Resume += Input.ActivateInput;
        Pause.SaveProgress += _state.SaveParams;
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
        Input.ChangeGun += _gunService.SetArsenal;
        Input.ChangeGun += _hud.OnChangedGun;
        Input.ChangeSkill += _hud.OnChangedSkill;
        Input.ChangeSkill += _skillsManager.SetSkill;
        Input.UseSkill += _skillsManager.UseSkill;

        _state.LevelUp += Enemies.OnLevelUp;
        _state.LevelUp += _hud.OnGameLevelUp;
        _state.PlayerLevelUp += _hud.OnPlayerLevelUp;
        _state.ChangePoints += _hud.OnChangedPoints;
        _state.ChangePoints += Stats.OnChangedPoints;
        _state.Disable += OnDisable;
    }

    private void Start() => _state.InitDependencies(_gunLoot, _gunService, Stats, _playerParams, _skillsManager);

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
        Input.ChangeGun -= _gunService.SetArsenal;

        Input.ChangeSkill -= _skillsManager.SetSkill;
        Input.UseSkill -= _skillsManager.UseSkill;
        Pause.SaveProgress -= _skillsManager.SaveParams;
    }
}
