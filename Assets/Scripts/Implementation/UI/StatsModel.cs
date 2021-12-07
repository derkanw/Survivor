using System.Collections.Generic;
using UnityEngine;
using System;

public class StatsModel : MonoBehaviour, IStatsModel
{
    public event Action Resume;
    public event Action<int> GetPoints;
    public event Action<Dictionary<StatsNames, int>> GetStats;

    [SerializeField] private GameObject StatsList;
    private GameObject _statsList;
    private IPointsService _pointsService;

    public void OnChangedPoints(int points) => _pointsService.Points = points;

    public void OnLooksStats() => _statsList.SetActive(true);

    public void SaveParams()
    {
        SaveSystem.Save(Tokens.StatsPoints, _pointsService.Points);
        SaveSystem.Save(Tokens.Stats, _pointsService.GetStats());
    }

    private void Start()
    {
        _statsList = Instantiate(StatsList, Vector3.zero, Quaternion.identity);
        _statsList.SetActive(false);
        _statsList.GetComponent<IButtonModel>().Resume += OnResume;
        _pointsService = _statsList.GetComponent<IPointsService>();
        _pointsService.Points = SaveSystem.Load<int>(Tokens.StatsPoints);
        GetPoints?.Invoke(_pointsService.Points);
        _pointsService.SetStats(SaveSystem.Load<Dictionary<StatsNames, int>>(Tokens.Stats));
        GetStats?.Invoke(_pointsService.GetStats());
    }

    private void OnResume()
    {
        _statsList.SetActive(false);
        GetPoints?.Invoke(_pointsService.Points);
        Resume?.Invoke();
        GetStats?.Invoke(_pointsService.GetStats());
    }
}
