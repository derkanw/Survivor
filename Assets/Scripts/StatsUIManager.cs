using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StatsUIManager : MonoBehaviour
{
    public event Action Resume;
    public event Action<int> GetPoints;
    public event Action<Dictionary<StatsNames, int>> GetStats;

    [SerializeField] private GameObject StatsList;
    private GameObject _statsList;
    private PointsManager _pointsManager;

    public void OnChangedPoints(int points) => _pointsManager.Points = points;

    public void OnLooksStats() => _statsList.SetActive(true);

    private void Start()
    {
        _statsList = Instantiate(StatsList, Vector3.zero, Quaternion.identity);
        _statsList.SetActive(false);
        _statsList.GetComponent<ButtonManager>().Resume += OnResume;
        _pointsManager = _statsList.GetComponent<PointsManager>();
    }

    private void OnResume()
    {
        _statsList.SetActive(false);
        GetPoints?.Invoke(_pointsManager.Points);
        Resume?.Invoke();
        GetStats?.Invoke(_pointsManager.UpdateStats());
    }
}
