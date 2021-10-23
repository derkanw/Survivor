using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] private float SmoothSpeed;

    private Vector3 _offset;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _toPlayer;
    private bool _isPlayerExist;

    private void Start()
    {
        _offset = transform.position;
        _isPlayerExist = true;
    }

    private void LateUpdate()
    {
        if (_isPlayerExist)
        {
            Vector3 targetPos = _toPlayer + _offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, SmoothSpeed * Time.deltaTime);
        }
    }

    public void ToPlayer(Vector3 position) => _toPlayer = position;

    public void ToStay() => _isPlayerExist = false;
}
