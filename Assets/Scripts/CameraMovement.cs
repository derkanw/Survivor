using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] [Range(0f, 1f)] private float SmoothSpeed;

    private Vector3 _offset;
    private Vector3 _velocity = Vector3.zero;

    private void Start()
    {
        _offset = transform.position;
    }

    private void LateUpdate()
    {
        if (Player != null)
        {
            Vector3 targetPos = Player.position + _offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, SmoothSpeed * Time.deltaTime);
        }
    }
}
