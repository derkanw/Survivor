using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 3f;

    private Rigidbody _rigidBody;
    private Vector3 _position = Vector3.zero, _offset;

    public void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _offset = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }

    public void Update()
    {
        _position.x = Input.GetAxisRaw("Horizontal");
        _position.z = Input.GetAxisRaw("Vertical");

        Vector3 mousePos = Input.mousePosition - _offset;
        Vector3 point = new Vector3(-transform.position.x, Screen.height / 2 - transform.position.z, 0);

        float angle = Vector3.Angle(mousePos, point);
        float sign = (mousePos.x < point.x) ? -1.0f : 1.0f;
        transform.rotation = Quaternion.Euler(0, sign * angle, 0);
    }

    public void FixedUpdate()
    {
        _rigidBody.velocity = _position * Speed;
    }
}
