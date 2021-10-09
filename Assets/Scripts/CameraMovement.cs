using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] private float SmoothSpeed;

    private Vector3 _offset;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _toPlayer;
    private Vector3 _toMouse;

    private void Start()
    {
        _offset = transform.position;
    }

    private void LateUpdate()
    {
        if (_toPlayer != Vector3.positiveInfinity)
        {
            Vector3 targetPos = _toPlayer + _offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, SmoothSpeed * Time.deltaTime);
        }
    }

    public void ToPlayer(Vector3 position) => _toPlayer = position;

    public void ToMouse(Vector3 position) => _toMouse = position;
}
