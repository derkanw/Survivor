using UnityEngine;
using System;
using System.Collections.Generic;

public class InputSystem : MonoBehaviour
{
    public event Action<Vector3> CursorMoved;
    public event Action<Vector3> ChangedPosition;
    public event Action<bool> CursorClicked;
    public event Action<bool> Reloading;
    public event Action<bool> UseSkill;
    public event Action<int> ChangeWeapon;
    public event Action<int> ChangeSkill;

    private bool _canUse;
    private int _weaponsCount;
    private int _skillsCount;
    private int _currentWeapon;
    private int _currentSkill;
    private List<int> _SkillsExists;

    public void DisableInput() => _canUse = false;
    public void ActivateInput() => _canUse = true;
    public void OnChangedSkillCount(int index, int count) => _SkillsExists[index] = count;
    public void SetWeaponCount(int value) => _weaponsCount = value;
    public void SetSkillsCount(int value)
    {
        _skillsCount = value;
        _SkillsExists = new List<int>(_skillsCount);
        for (int index = 0; index < _skillsCount; ++index)
            _SkillsExists.Add(0);
    }

    private void RotatePlayer()
    {
        Vector3 direction = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit rh))
            direction = rh.point;
        direction.y = 0;
        CursorMoved?.Invoke(direction);
    }

    private void MovePlayer()
    {
        Vector3 position = Vector3.zero;
        position.x = Input.GetAxisRaw("Horizontal");
        position.z = Input.GetAxisRaw("Vertical");
        ChangedPosition?.Invoke(position);
    }

    private void GetSkillInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5) && _skillsCount >= 1)
        {
            _currentSkill = 0;
            ChangeSkill?.Invoke(_currentSkill);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) && _skillsCount >= 2)
        {
            _currentSkill = 1;
            ChangeSkill?.Invoke(_currentSkill);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7) && _skillsCount >= 3)
        {
            _currentSkill = 2;
            ChangeSkill?.Invoke(_currentSkill);
        }
    }

    private void GetWeaponInput()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (_currentWeapon < _weaponsCount - 1)
                ++_currentWeapon;
            else
                _currentWeapon = 0;
            ChangeWeapon?.Invoke(_currentWeapon);
        }
            
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if(_currentWeapon > 0)
                --_currentWeapon;
            else
                _currentWeapon = _weaponsCount - 1;
            ChangeWeapon?.Invoke(_currentWeapon);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && _weaponsCount >= 1)
        {
            _currentWeapon = 0;
            ChangeWeapon?.Invoke(_currentWeapon);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && _weaponsCount >= 2)
        {
            _currentWeapon = 1;
            ChangeWeapon?.Invoke(_currentWeapon);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && _weaponsCount >= 3)
        {
            _currentWeapon = 2;
            ChangeWeapon?.Invoke(_currentWeapon);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && _weaponsCount >= 4)
        {
            _currentWeapon = 3;
            ChangeWeapon?.Invoke(_currentWeapon);
        }
    }

    private void Awake()
    {
        _currentSkill = -1;
        _canUse = true;
    }

    private void Update()
    {
        if (!_canUse) return;
        MovePlayer();
        RotatePlayer();
        GetWeaponInput();
        Reloading?.Invoke(Input.GetKeyDown(KeyCode.R));
        CursorClicked?.Invoke(Input.GetKey(KeyCode.Mouse0));
        GetSkillInput();
        if (_currentSkill >= 0 && _SkillsExists[_currentSkill] > 0)
            UseSkill?.Invoke(Input.GetKeyDown(KeyCode.Q));
    }
}
