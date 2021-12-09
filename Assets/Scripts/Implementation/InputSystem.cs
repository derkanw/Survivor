using UnityEngine;
using System;
using System.Collections.Generic;

public class InputSystem : MonoBehaviour, IInputSystem
{
    public event Action<Vector3> CursorMoved;
    public event Action<Vector3> ChangedPosition;
    public event Action<bool> CursorClicked;
    public event Action<bool> Reloading;
    public event Action<bool> UseSkill;
    public event Action<int> ChangeGun;
    public event Action<int> ChangeSkill;

    private bool _canUse;
    private int _gunCount;
    private int _skillCount;
    private int _currentGun;
    private int _currentSkill;
    private List<int> _skillsExist;

    public void DisableInput() => _canUse = false;
    public void ActivateInput() => _canUse = true;
    public void OnChangedSkillCount(int index, int count) => _skillsExist[index] = count;
    public void SetGunCount(int value) => _gunCount = value;
    public void SetSkillCount(int value)
    {
        _skillCount = value;
        _skillsExist = new List<int>(_skillCount);
        for (int index = 0; index < _skillCount; ++index)
            _skillsExist.Add(0);
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
        if (Input.GetKeyDown(KeyCode.Alpha5) && _skillCount >= 1)
        {
            _currentSkill = 0;
            ChangeSkill?.Invoke(_currentSkill);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) && _skillCount >= 2)
        {
            _currentSkill = 1;
            ChangeSkill?.Invoke(_currentSkill);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7) && _skillCount >= 3)
        {
            _currentSkill = 2;
            ChangeSkill?.Invoke(_currentSkill);
        }
    }

    private void GetGunInput()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (_currentGun < _gunCount - 1)
                ++_currentGun;
            else
                _currentGun = 0;
            ChangeGun?.Invoke(_currentGun);
        }
            
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if(_currentGun > 0)
                --_currentGun;
            else
                _currentGun = _gunCount - 1;
            ChangeGun?.Invoke(_currentGun);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && _gunCount >= 1)
        {
            _currentGun = 0;
            ChangeGun?.Invoke(_currentGun);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && _gunCount >= 2)
        {
            _currentGun = 1;
            ChangeGun?.Invoke(_currentGun);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && _gunCount >= 3)
        {
            _currentGun = 2;
            ChangeGun?.Invoke(_currentGun);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && _gunCount >= 4)
        {
            _currentGun = 3;
            ChangeGun?.Invoke(_currentGun);
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
        GetGunInput();
        Reloading?.Invoke(Input.GetKeyDown(KeyCode.R));
        CursorClicked?.Invoke(Input.GetKey(KeyCode.Mouse0));
        GetSkillInput();
        if (_currentSkill >= 0 && _skillsExist[_currentSkill] > 0)
            UseSkill?.Invoke(Input.GetKeyDown(KeyCode.Q));
    }
}
