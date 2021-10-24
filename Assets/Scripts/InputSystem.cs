using UnityEngine;
using System;

public class InputSystem : MonoBehaviour
{
    public event Action<Vector3> CursorMoved;
    public event Action<Vector3> ChangedPosition;
    public event Action<bool> MouseClicked;
    public event Action<bool> Reloading;

    private bool _isPaused = false;

    public void OnPause() => _isPaused = true;

    public void OnResume() => _isPaused = false;

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

    private void Update()
    {
        if (_isPaused) return;
        MovePlayer();
        RotatePlayer();
        Reloading?.Invoke(Input.GetKeyDown(KeyCode.R));
        MouseClicked?.Invoke(Input.GetKeyDown(KeyCode.Mouse0));
    }
}
