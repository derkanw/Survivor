using System;
using UnityEngine;

public interface IInputSystem
{
    public event Action<Vector3> CursorMoved;
    public event Action<Vector3> ChangedPosition;
    public event Action<bool> CursorClicked;
    public event Action<bool> Reloading;
    public event Action<bool> UseSkill;
    public event Action<int> ChangeGun;
    public event Action<int> ChangeSkill;

    public void DisableInput();
    public void ActivateInput();
    public void OnChangedSkillCount(int index, int count);
    public void SetGunCount(int value);
    public void SetSkillCount(int value);
}