using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform Player;

    [SerializeField]
    [Range(0f, 1f)]
    private float SmoothSpeed = 0.125f;

    private Vector3 _offset, _velocity = Vector3.zero;

    public void Start()
    {
        _offset = transform.position;
    }

    public void LateUpdate()
    {
        Vector3 targetPos = Player.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, SmoothSpeed * Time.deltaTime);
    }
}
