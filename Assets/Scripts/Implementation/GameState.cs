using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour, IGameState
{
    public event Action<int> PlayerLevelUp;
    public event Action<int> LevelUp;
    public event Action<int> ChangePoints;
    public event Action Disable;

    [SerializeField] private int LevelCount;
    [SerializeField] private Stat PointsTarget;
    [SerializeField] private GameObject MenuUI;

    private float _points;
    private int _playerLevel;
    private int _gameLevel;

    private IGunService _gunService;
    private IGunLoot _gunLoot;
    private IStatsModel _stats;
    private IPlayer _playerParams;
    private ISkillService _skills;
    private IMenuModel _menu;

    public void InitDependencies(IGunLoot loot, IGunService manager, IStatsModel stats, IPlayer player, ISkillService skills)
    {
        _gunLoot = loot;
        _gunService = manager;
        _stats = stats;
        _playerParams = player;
        _skills = skills;
    }

    public void Notify() => _gunService.SetNewGun();

    public void OnPlayerDied()
    {
        Disable?.Invoke();
        _menu.SetContinueAbility(false);
        SaveSystem.DeleteAll();
    }

    public void OnSetPoints(float points)
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

    public void SaveParams()
    {
        SaveSystem.Save<int>(Tokens.PlayerLevel, _playerLevel);
        SaveSystem.Save<int>(Tokens.GameLevel, _gameLevel);
        SaveSystem.Save<float>(Tokens.Points, _points);
        Disable?.Invoke();
    }

    public void PrepareNextLevel()
    {
        ++_gameLevel;
        SaveParams();
        _stats.SaveParams();
        _playerParams.SaveParams();
        _skills.SaveParams();
        _gunLoot.SpawnLoot();
        _playerParams.DestroyPlayer();
    }

    public void ChangeScene() => SceneManager.LoadScene(_gameLevel >= LevelCount ? Tokens.MainMenu : SceneManager.GetActiveScene().name);

    private void Awake()
    {
        PointsTarget.Init();
        _menu = MenuUI.GetComponent<IMenuModel>();
        _menu.SetContinueAbility(true);
    }

    private void Start()
    {
        _gameLevel = SaveSystem.Load<int>(Tokens.GameLevel);
        _points = SaveSystem.Load<float>(Tokens.Points);
        PlayerLevelUp?.Invoke(_playerLevel);
        PointsTarget.Modify(_playerLevel);
        LevelUp?.Invoke(_gameLevel);
    }
}
