using System;
using UnityEngine;

public interface IGunService
{
    public event Action<int> GetArsenalSize;
    public event Action<int> GetGunCount;
    public event Action<float> ChangedBulletCount;
    public event Action<float> ChangedClipSize;
    public event Action<float> Reloading;

    public void LookTo(Vector3 direction);
    public void SetShooting(bool value);
    public void SetReloading(bool value);
    public void SetNewGun();
    public void SetGunParams(float agility, float power);
    public void SetArsenal(int index);
}