using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public event Action<float> OnChangedHP;
    public event Action<Vector3> OnMoved;
    public event Action OnDied;

    [Range(0f, 100f)] public float MaxHP;
    [SerializeField] [Range(0f, 10f)] private float Speed;

    private Rigidbody _rigidBody;
    private Animator _animator;
    private float _hp;
    private Vector3 _position;

    private void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _animator = gameObject.GetComponent<Animator>();
        _hp = MaxHP;
    }

    public void ToMove(Vector3 position) => _position = position * Speed;

    public void ToLook(Vector3 direction) => transform.LookAt(direction);

    private void FixedUpdate()
    {
        if (_position == Vector3.zero)
            _animator.SetBool("isMoving", false);
        else
        {
            _animator.SetBool("isMoving", true);
            _rigidBody.MovePosition(transform.position + _position * Time.fixedDeltaTime);
        }
        OnMoved?.Invoke(transform.position);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            _hp -= collider.gameObject.GetComponent<BaseEnemy>().Power;
            OnChangedHP?.Invoke(_hp / MaxHP);
        }
        if (_hp <= 0)
        {
            OnDied?.Invoke();
            Destroy(gameObject);
        }
    }
}
