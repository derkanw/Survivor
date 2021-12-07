using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour, IGameState
{
    public event Action<int> PlayerLevelUp;
    public event Action<int> LevelUp;
    public event Action<int> ChangePoints;
    public event Action Disable;

    [SerializeField] private uint LevelCount;
    [SerializeField] private Stat PointsTarget;
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private MainMenuManager Menu;

    private float _points;
    private int _playerLevel;
    private int _gameLevel;

    private IGunService _gunService;
    private IGunLoot _gunLoot;
    private StatsUIManager _stats;
    private Player _playerParams;
    private SkillsManager _skills;

    public void InitDependencies(IGunLoot loot, IGunService manager, StatsUIManager stats, Player player, SkillsManager skills)
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
        Menu.SetActive(false);
        Instantiate(GameOverUI, Vector3.zero, Quaternion.identity);
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

    public void LoadNextLevel()
    {
        ++_gameLevel;
        SaveParams();
        _stats.SaveStats();
        _playerParams.SaveParams();
        _skills.SaveParams();
        _gunLoot.SpawnLoot(_gameLevel >= LevelCount ? Tokens.MainMenu : SceneManager.GetActiveScene().name);
        Destroy(_playerParams.gameObject);
    }

    private void Awake()
    {
        PointsTarget.Init();
        Menu.SetActive(true);
    }

    private void Start()
    {
        _playerLevel = SaveSystem.Load<int>(Tokens.PlayerLevel);
        _gameLevel = SaveSystem.Load<int>(Tokens.GameLevel);
        _points = SaveSystem.Load<float>(Tokens.Points);
        PlayerLevelUp?.Invoke(_playerLevel);
        PointsTarget.Modify(_playerLevel);
        LevelUp?.Invoke(_gameLevel);
    }
}
