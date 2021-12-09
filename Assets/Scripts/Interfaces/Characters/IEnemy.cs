using System;
using System.Collections;

public interface IEnemy : IMovable, IDamageable
{
    public event Action<IEnemy, float> EnemyDied;
    public IEnumerator DecreaseSpeed(int inc, float time);
    public void OnLevelUp(int level);
}