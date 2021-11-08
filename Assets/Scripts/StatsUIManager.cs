using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StatsUIManager : MonoBehaviour
{
    public event Action Resume;
    public event Action<int> GetPoints;
    public event Action<Dictionary<StatsNames, int>> GetStats;

    [SerializeField] private GameObject StatsList;
    private GameObject _statsList;
    private PointsManager _pointsManager;
    private readonly string _pointsField = "PointsForStats";
    private readonly string _statsField = "PlayerStats";

    public void OnChangedPoints(int points) => _pointsManager.Points = points;

    public void OnLooksStats() => _statsList.SetActive(true);

    public void SaveStats()
    {
        SaveSystem.Save(_pointsField, _pointsManager.Points);
        SaveSystem.Save(_statsField, _pointsManager.GetStats());
    }

    private void Start()
    {
        _statsList = Instantiate(StatsList, Vector3.zero, Quaternion.identity);
        _statsList.SetActive(false);
        _statsList.GetComponent<ButtonManager>().Resume += OnResume;
        _pointsManager = _statsList.GetComponent<PointsManager>();
        _pointsManager.Points = SaveSystem.Load<int>(_pointsField);
        GetPoints?.Invoke(_pointsManager.Points);
        _pointsManager.SetStats(SaveSystem.Load<Dictionary<StatsNames, int>>(_statsField));
        GetStats?.Invoke(_pointsManager.GetStats());
    }

    private void OnResume()
    {
        _statsList.SetActive(false);
        GetPoints?.Invoke(_pointsManager.Points);
        Resume?.Invoke();
        GetStats?.Invoke(_pointsManager.GetStats());
    }
}
