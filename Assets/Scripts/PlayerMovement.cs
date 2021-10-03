using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 3f;
    [HideInInspector] public Vector3 direction;

    private Rigidbody _rigidBody;
    private Vector3 _position;

    public void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    public void Update()
    {
        _position.x = Input.GetAxisRaw("Horizontal");
        _position.z = Input.GetAxisRaw("Vertical");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit rh))
            direction = rh.point;
        direction.y = 0;
        transform.LookAt(direction);
    }

    public void FixedUpdate()
    {
        _rigidBody.MovePosition(transform.position + _position * Speed * Time.fixedDeltaTime);
    }
}
