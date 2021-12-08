using System;
using System.Collections.Generic;

public interface IStatsModel : IModel
{
    public event Action<int> GetPoints;
    public event Action<Dictionary<StatsNames, int>> GetStats;

    public void OnChangedPoints(int points);
    public void SaveParams();
}