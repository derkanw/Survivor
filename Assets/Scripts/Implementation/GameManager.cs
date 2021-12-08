using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CameraMovement MainCamera;
    [SerializeField] private GameObject LevelHUD;
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private GameObject Loot;
    [SerializeField] private GameObject Environment;

    private IGameState _state;
    private IEnemyService _enemies;
    private IInputSystem _input;
    private ILevelView _hud;
    private IButtonModel _buttonModel;
    private IPauseModel _pause;
    private IStatsModel _stats;
    private IGunLoot _gunLoot;

    private IPlayer _playerParams;
    private IGunService _gunService;
    private ISkillService _skillService;

    private void Awake()
    {
        _state = gameObject.GetComponent<IGameState>();
        _stats = gameObject.GetComponent<IStatsModel>();
        _pause = gameObject.GetComponent<IPauseModel>();
        _input = gameObject.GetComponent<IInputSystem>();
        _enemies = gameObject.GetComponent<IEnemyService>();

        Instantiate(Environment);

        var levelHud = Instantiate(LevelHUD, Vector3.zero, Quaternion.identity);
        levelHud.GetComponent<Canvas>().sortingOrder = levelHud.transform.childCount + 1;
        _hud = levelHud.GetComponent<ILevelView>();
        _buttonModel = levelHud.GetComponent<IButtonModel>();

        var lootUI = Instantiate(Loot, Vector3.zero, Quaternion.identity);
        _gunLoot = lootUI.GetComponent<IGunLoot>();

        var player = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        _playerParams = player.GetComponent<IPlayer>();
        _gunService = player.GetComponent<IGunService>();
        _skillService = player.GetComponent<ISkillService>();

        _playerParams.ChangedHP += _hud.ChangeHealthBar;
        _playerParams.Moved += MainCamera.MoveTo;
        _playerParams.Moved += _enemies.MoveEnemiesTo;
        _playerParams.Died += MainCamera.Stay;
        _playerParams.Died += _enemies.NotifyEnemies;
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
        _gunLoot.LevelEnd += _hud.FadeOut;

        _hud.SceneFinished += _state.ChangeScene;

        _skillService.GetSkillCount += _input.SetSkillCount;
        _skillService.ChangedSkillCount += _input.OnChangedSkillCount;
        _skillService.ChangedSkillCount += _hud.ViewSkill;
        _skillService.ChangedSkillReload += _hud.ChangeSkillReloadingBar;

        _enemies.ChangedKilledCount += _hud.ChangeBulletBar;
        _enemies.SetPoints += _state.OnSetPoints;
        _enemies.PlayerWin += _state.PrepareNextLevel;
        _enemies.PlayerWin += _input.DisableInput;

        _buttonModel.Pause += _pause.ViewModel;
        _buttonModel.Pause += _input.DisableInput;
        _buttonModel.LooksStats += _input.DisableInput;
        _buttonModel.LooksStats += _stats.ViewModel;

        _pause.Resume += _input.ActivateInput;
        _pause.SaveProgress += _state.SaveParams;
        _pause.SaveProgress += _stats.SaveParams;
        _pause.SaveProgress += _enemies.SaveParams;
        _pause.SaveProgress += _playerParams.SaveParams;
        _pause.SaveProgress += _skillService.SaveParams;

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
        _input.ChangeSkill += _skillService.SetSkill;
        _input.UseSkill += _skillService.UseSkill;

        _state.LevelUp += _enemies.OnLevelUp;
        _state.LevelUp += _hud.OnGameLevelUp;
        _state.PlayerLevelUp += _hud.OnPlayerLevelUp;
        _state.ChangePoints += _hud.OnChangedPoints;
        _state.ChangePoints += _stats.OnChangedPoints;
        _state.Disable += OnDisable;
    }

    private void Start() => _state.InitDependencies(_gunLoot, _gunService, _stats, _playerParams, _skillService);

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

        _input.ChangeSkill -= _skillService.SetSkill;
        _input.UseSkill -= _skillService.UseSkill;
        _pause.SaveProgress -= _skillService.SaveParams;
    }
}
