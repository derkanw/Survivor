using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Range(0f, 100f)] public float HP;
    [HideInInspector] public Vector3 direction;
    [SerializeField] [Range(0f, 10f)] private float Speed;

    private Rigidbody _rigidBody;
    private Animator _animator;
    private Vector3 _position;

    private void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        _position.x = Input.GetAxisRaw("Horizontal");
        _position.z = Input.GetAxisRaw("Vertical");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit rh))
            direction = rh.point;
        direction.y = 0;
        transform.LookAt(direction);
    }

    private void FixedUpdate()
    {
        if (_position == Vector3.zero)
            _animator.SetBool("isMoving", false);
        else
        {
            _animator.SetBool("isMoving", true);
            _rigidBody.MovePosition(transform.position + _position * Speed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
            HP -= collider.gameObject.GetComponent<BaseEnemy>().Power;
        if (HP == 0)
            Destroy(gameObject);
    }
}
