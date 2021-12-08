using System;
using UnityEngine;

public interface IEnemyService
{
    public event Action<float> ChangedKilledCount;
    public event Action<float> SetPoints;
    public event Action PlayerWin;

    public void MoveEnemiesTo(Vector3 position);
    public void NotifyEnemies();
    public void OnLevelUp(int level);
    public void SaveParams();
}