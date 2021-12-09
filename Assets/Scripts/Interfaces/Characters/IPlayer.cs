using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer : IMovable, IDamageable
{
    public event Action<float> ChangedHP;
    public event Action<Vector3> Moved;
    public event Action Died;

    public void LookTo(Vector3 direction);
    public void OnLevelUp(Dictionary<StatsNames, int> stats);
    public void Heal(float incHP);
    public void SpeedUp(int incSpeed, float time);
    public void PowerUp(float power, float time);
    public void SaveParams();
}