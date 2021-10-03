using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(0f, 10f)] public float Speed;
    [Range(0f, 100f)] public float HP;
    [HideInInspector] public Vector3 direction;

    private Rigidbody _rigidBody;
    private Animator _animator;
    private Vector3 _position;

    public void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _animator = gameObject.GetComponent<Animator>();
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
        if (_position == Vector3.zero)
            _animator.SetBool("isMoving", false);
        else
        {
            _animator.SetBool("isMoving", true);
            _rigidBody.MovePosition(transform.position + _position * Speed * Time.fixedDeltaTime);
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
            HP -= collider.gameObject.GetComponent<BaseEnemy>().Power;
        if (HP == 0)
            Destroy(gameObject);
    }
}
