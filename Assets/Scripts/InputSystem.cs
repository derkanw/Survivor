using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputSystem : MonoBehaviour
{
    public event Action<Vector3> OnMouseMoved;
    public event Action<Vector3> OnChangedPosition;
    public event Action<bool> OnMouseClicked;
    public event Action<bool> OnReloadingClicked;

    private void RotatePlayer()
    {
        Vector3 direction = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit rh))
            direction = rh.point;
        direction.y = 0;
        OnMouseMoved?.Invoke(direction);
    }

    private void MovePlayer()
    {
        Vector3 position = Vector3.zero;
        position.x = Input.GetAxisRaw("Horizontal");
        position.z = Input.GetAxisRaw("Vertical");
        OnChangedPosition?.Invoke(position);
    }

    private void Update()
    {
        MovePlayer();
        RotatePlayer();
        OnMouseClicked?.Invoke(Input.GetKeyDown(KeyCode.Mouse0));
        OnReloadingClicked?.Invoke(Input.GetKeyDown(KeyCode.R));
    }
}
