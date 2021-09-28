using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 3f;
    internal Vector3 Direction;

    private Rigidbody _rigidBody;
    private Vector3 _position, _offset;

    public void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        Vector3 cameraPos = Camera.main.transform.position;
        _offset = new Vector3(0, 0, cameraPos.magnitude);
    }

    public void Update()
    {
        _position.x = Input.GetAxisRaw("Horizontal");
        _position.z = Input.GetAxisRaw("Vertical");

        Direction = Camera.main.ScreenToWorldPoint(Input.mousePosition + _offset);
        Direction.y = 0;
        transform.LookAt(Direction);
    }

    public void FixedUpdate()
    {
        _rigidBody.MovePosition(transform.position + _position * Speed * Time.fixedDeltaTime);
    }
}
