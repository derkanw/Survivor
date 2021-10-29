using UnityEngine;
using System;

public class InputSystem : MonoBehaviour
{
    public event Action<Vector3> CursorMoved;
    public event Action<Vector3> ChangedPosition;
    public event Action<bool> CursorClicked;
    public event Action<bool> ReloadingClicked;
    public event Action<int> ChangeWeapon;

    private bool _isPaused = false;
    private int _weaponsCount;
    private int _currentIndex;

    public void OnPause() => _isPaused = true;

    public void OnResume() => _isPaused = false;

    public void SetWeaponCount(int value) => _weaponsCount = value;

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

    private void GetWeaponInput()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (_currentIndex < _weaponsCount - 1)
                ++_currentIndex;
            else
                _currentIndex = 0;
            ChangeWeapon?.Invoke(_currentIndex);
        }
            
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if(_currentIndex > 0)
                --_currentIndex;
            else
                _currentIndex = _weaponsCount - 1;
            ChangeWeapon?.Invoke(_currentIndex);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && _weaponsCount >= 1)
        {
            _currentIndex = 0;
            ChangeWeapon?.Invoke(_currentIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && _weaponsCount >= 2)
        {
            _currentIndex = 1;
            ChangeWeapon?.Invoke(_currentIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && _weaponsCount >= 3)
        {
            _currentIndex = 2;
            ChangeWeapon?.Invoke(_currentIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && _weaponsCount >= 4)
        {
            _currentIndex = 3;
            ChangeWeapon?.Invoke(_currentIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && _weaponsCount >= 5)
        {
            _currentIndex = 4;
            ChangeWeapon?.Invoke(_currentIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) && _weaponsCount >= 6)
        {
            _currentIndex = 5;
            ChangeWeapon?.Invoke(_currentIndex);
        }
    }

    private void Update()
    {
        if (_isPaused) return;
        MovePlayer();
        RotatePlayer();
        GetWeaponInput();
        ReloadingClicked?.Invoke(Input.GetKeyDown(KeyCode.R));
        CursorClicked?.Invoke(Input.GetKey(KeyCode.Mouse0));
    }
}
