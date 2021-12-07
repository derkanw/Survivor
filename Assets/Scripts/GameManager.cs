using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemiesManager Enemies;

    [SerializeField] private CameraMovement MainCamera;
    [SerializeField] private GameObject LevelHUD;
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private GameObject Loot;
    [SerializeField] private GameObject Environment;

    private IGameState _state;
    private IInputSystem _input;
    private ILevelView _hud;
    private IButtonModel _buttonModel;
    private IPauseModel _pause;
    private IStatsModel _stats;
    private IGunLoot _gunLoot;

    private Player _playerParams;
    private IGunService _gunService;
    private SkillsManager _skillsManager;

    private void Awake()
    {
        _state = gameObject.GetComponent<IGameState>();
        _stats = gameObject.GetComponent<IStatsModel>();
        _pause = gameObject.GetComponent<IPauseModel>();
        _input = gameObject.GetComponent<IInputSystem>();

        Instantiate(Environment);
        var levelHud = Instantiate(LevelHUD, Vector3.zero, Quaternion.identity);
        _hud = levelHud.GetComponent<ILevelView>();
        _buttonModel = levelHud.GetComponent<IButtonModel>();

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
        _playerParams.Died += _buttonModel.DisableButtons;
        _playerParams.Died += _input.DisableInput;

        _gunService.ChangedClipSize += _hud.OnChangedClipSize;
        _gunService.ChangedBulletCount += _hud.OnChangedBulletCount;
        _gunService.Reloading += _hud.ChangeReloadBar;
        _gunService.GetGunCount += _hud.ViewGuns;
        _gunService.GetGunCount += _input.SetGunCount;
        _gunService.GetArsenalSize += _gunLoot.SetArsenalSize;

        _gunLoot.LootSpawned += _state.Notify;

        _skillsManager.GetSkillsCount += _input.SetSkillCount;
        _skillsManager.ChangedSkillCount += _input.OnChangedSkillCount;
        _skillsManager.ChangedSkillCount += _hud.ViewSkill;
        _skillsManager.ChangedSkillReload += _hud.ChangeSkillReloadingBar;

        Enemies.ChangedKilledCount += _hud.ChangeBulletBar;
        Enemies.SetPoints += _state.OnSetPoints;
        Enemies.PlayerWin += _state.LoadNextLevel;
        Enemies.PlayerWin += _input.DisableInput;

        _buttonModel.Pause += _pause.OnPause;
        _buttonModel.Pause += _input.DisableInput;
        _buttonModel.LooksStats += _input.DisableInput;
        _buttonModel.LooksStats += _stats.OnLooksStats;

        _pause.Resume += _input.ActivateInput;
        _pause.SaveProgress += _state.SaveParams;
        _pause.SaveProgress += _stats.SaveParams;
        _pause.SaveProgress += Enemies.SaveParams;
        _pause.SaveProgress += _playerParams.SaveParams;
        _pause.SaveProgress += _skillsManager.SaveParams;

        _stats.Resume += _input.ActivateInput;
        _stats.GetPoints += _hud.OnChangedPoints;
        _stats.GetStats += _playerParams.OnLevelUp;

        _input.ChangedPosition += _playerParams.MoveTo;
        _input.CursorMoved += _playerParams.LookTo;
        _input.CursorMoved += _gunService.LookTo;
        _input.CursorClicked += _gunService.SetShooting;
        _input.Reloading += _gunService.SetReloading;
        _input.ChangeGun += _gunService.SetArsenal;
        _input.ChangeGun += _hud.OnChangedGun;
        _input.ChangeSkill += _hud.OnChangedSkill;
        _input.ChangeSkill += _skillsManager.SetSkill;
        _input.UseSkill += _skillsManager.UseSkill;

        _state.LevelUp += Enemies.OnLevelUp;
        _state.LevelUp += _hud.OnGameLevelUp;
        _state.PlayerLevelUp += _hud.OnPlayerLevelUp;
        _state.ChangePoints += _hud.OnChangedPoints;
        _state.ChangePoints += _stats.OnChangedPoints;
        _state.Disable += OnDisable;
    }

    private void Start() => _state.InitDependencies(_gunLoot, _gunService, _stats, _playerParams, _skillsManager);

    private void OnDisable()
    {
        _input.CursorMoved -= _playerParams.LookTo;
        _input.ChangedPosition -= _playerParams.MoveTo;
        _stats.GetStats -= _playerParams.OnLevelUp;
        _pause.SaveProgress -= _playerParams.SaveParams;

        _gunLoot.LootSpawned -= _gunService.SetNewGun;
        _input.CursorMoved -= _gunService.LookTo;
        _input.CursorClicked -= _gunService.SetShooting;
        _input.Reloading -= _gunService.SetReloading;
        _input.ChangeGun -= _gunService.SetArsenal;

        _input.ChangeSkill -= _skillsManager.SetSkill;
        _input.UseSkill -= _skillsManager.UseSkill;
        _pause.SaveProgress -= _skillsManager.SaveParams;
    }
}
