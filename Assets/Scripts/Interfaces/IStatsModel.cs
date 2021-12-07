using System;
using System.Collections.Generic;

public interface IStatsModel
{
    public event Action Resume;
    public event Action<int> GetPoints;
    public event Action<Dictionary<StatsNames, int>> GetStats;

    public void OnChangedPoints(int points);
    public void OnLooksStats();
    public void SaveParams();
}