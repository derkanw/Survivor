using System;

public interface IGameState
{
    public event Action<int> PlayerLevelUp;
    public event Action<int> LevelUp;
    public event Action<int> ChangePoints;
    public event Action Disable;

    public void InitDependencies(IGunLoot loot, IGunService manager, IStatsModel stats, IPlayer player, ISkillService skills);
    public void Notify();
    public void OnPlayerDied();
    public void OnSetPoints(float points);
    public void SaveParams();
    public void PrepareNextLevel();
    public void ChangeScene();
}