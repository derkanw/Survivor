using System;
using UnityEngine;

public interface IGun
{
    public event Action<float> Reloading;
    public event Action Shooting;

    public void SetShooting(bool value);
    public void SetReloading(bool value);
    public void SetParams(float incAgility, float incPower);
    public float GetBulletsCount();
    public float GetClipSize();
    public void LookTo(Vector3 direction);
    public void StopReloading();
}