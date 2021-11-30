using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private uint LevelsCount;
    [SerializeField] private Stat PointsTarget;

    [SerializeField] private StatsUIManager Stats;
    [SerializeField] private EnemiesManager Enemies;
    [SerializeField] private InputSystem Input;
    [SerializeField] private PauseUIManager Pause;
    [SerializeField] private MainMenuManager Menu;

    [SerializeField] private CameraMovement MainCamera;
    [SerializeField] private GameObject LevelHUD;
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private GameObject Loot;

    private event Action<int> PlayerLevelUp;
    private event Action<int> LevelUp;
    private event Action<int> ChangePoints;

    private Player _playerParams;
    private WeaponsManager _weaponsManager;
    private SkillsManager _skillsManager;
    private LevelHUDManager _hud;
    private ButtonManager _buttonManager;
    private WeaponLoot _weaponLoot;
    private float _points;
    private int _playerLevel;
    private int _gameLevel;

    private void Awake()
    {
        // TODO: load environment
        Menu.SetActive(true);
        PointsTarget.Init();

        var levelHud = Instantiate(LevelHUD, Vector3.zero, Quaternion.identity);
        _hud = levelHud.GetComponent<LevelHUDManager>();

        Enemies.ChangedKilledCount += _hud.ChangeBulletBar;
        Enemies.SetPoints += OnSetPoints;
        Enemies.PlayerWin += LoadNextLevel;

        var player = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        _playerParams = player.GetComponent<Player>();
        _playerParams.ChangedHP += _hud.ChangeHealthBar;
        _playerParams.Moved += MainCamera.Move;
        _playerParams.Moved += Enemies.MoveEnemiesTo;
        _playerParams.Died += OnPlayerDied;
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
        _weaponLoot.LootSpawned += Notify;


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

        Pause.SaveProgress += SaveParams;
        Pause.SaveProgress += Stats.SaveStats;
        Pause.SaveProgress += Enemies.SaveParams;

        PlayerLevelUp += _hud.OnPlayerLevelUp;
        ChangePoints += _hud.OnChangedPoints;

        ChangePoints += Stats.OnChangedPoints;
        Stats.GetPoints += _hud.OnChangedPoints;
        Stats.GetStats += _playerParams.OnLevelUp;
        LevelUp += Enemies.OnLevelUp;
        LevelUp += _hud.OnGameLevelUp;
    }

    private void Notify() => _weaponsManager.SetNewWeapon();
    
    private void Start()
    {
        _playerLevel = SaveSystem.Load<int>(Tokens.PlayerLevel);
        _gameLevel = SaveSystem.Load<int>(Tokens.GameLevel);
        _points = SaveSystem.Load<float>(Tokens.Points);
        PlayerLevelUp?.Invoke(_playerLevel);
        PointsTarget.Modify(_playerLevel);
        LevelUp?.Invoke(_gameLevel);
    }

    private void OnDisable()
    {
        Enemies.PlayerWin -= LoadNextLevel;
        Enemies.ChangedKilledCount -= _hud.ChangeBulletBar;
        Enemies.SetPoints -= OnSetPoints;

        _playerParams.ChangedHP -= _hud.ChangeHealthBar;
        _playerParams.Moved -= MainCamera.Move;
        _playerParams.Moved -= Enemies.MoveEnemiesTo;
        _playerParams.Died -= OnPlayerDied;
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

        Pause.SaveProgress -= SaveParams;
        Pause.SaveProgress -= Stats.SaveStats;
        Pause.SaveProgress -= Enemies.SaveParams;
        Pause.Resume -= Input.OnResume;
        Stats.Resume -= Input.OnResume;

        _buttonManager.Pause -= Pause.OnPause;
        _buttonManager.Pause -= Input.OnPause;
        _buttonManager.LooksStats -= Stats.OnLooksStats;
        _buttonManager.LooksStats -= Input.OnPause;

        PlayerLevelUp -= _hud.OnPlayerLevelUp;
        ChangePoints -= _hud.OnChangedPoints;

        ChangePoints -= Stats.OnChangedPoints;
        Stats.GetPoints -= _hud.OnChangedPoints;
        Stats.GetStats -= _playerParams.OnLevelUp;

        LevelUp -= Enemies.OnLevelUp;
        LevelUp -= _hud.OnGameLevelUp;
    }

    // TODO: game state class

    private void OnPlayerDied()
    {
        OnDisable();
        Menu.SetActive(false);
        Instantiate(GameOverUI, Vector3.zero, Quaternion.identity);
        SaveSystem.DeleteAll();
    }

    private void OnSetPoints(float points)
    {
        _points += points;
        if (_points >= PointsTarget.Value)
        {
            AudioManager.PlaySound(SoundNames.PlayerLevel);
            ++_playerLevel;
            ChangePoints?.Invoke(_playerLevel);
            PlayerLevelUp?.Invoke(_playerLevel);
            PointsTarget.Modify(_playerLevel);
            _points = 0;
        }
    }

    private void SaveParams()
    {
        SaveSystem.Save<int>(Tokens.PlayerLevel, _playerLevel);
        SaveSystem.Save<int>(Tokens.GameLevel, _gameLevel);
        SaveSystem.Save<float>(Tokens.Points, _points);
        OnDisable();
    }

    private void LoadNextLevel()
    {
        ++_gameLevel;
        SaveParams();
        Stats.SaveStats();
        _weaponLoot.SpawnLoot(_gameLevel >= LevelsCount ? Tokens.MainMenu : SceneManager.GetActiveScene().name);
    }
}
