using System.Collections.Generic;

public interface IPointsService
{
    public int Points { get; set; }

    public Dictionary<StatsNames, int> GetStats();
    public void SetStats(Dictionary<StatsNames, int> stats);
}