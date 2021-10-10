using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BaseEnemy : MonoBehaviour
{
    public event Action<BaseEnemy> OnEnemyDied;

    [Range(0f, 100f)] public float MaxHP;
    [Range(0f, 100f)] public float Power;
    [SerializeField] [Range(0f, 10f)] private float Speed;

    private Rigidbody _rigidBody;
    private Vector3 _position;
    private Vector3 _targetPosition;
    private Animator _animator;
    private Image _healthBar;
    private float _hp;
    private bool _isPlayerExist;

    private void Start()
    {
        _hp = MaxHP;
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _animator = gameObject.GetComponent<Animator>();
        _healthBar = transform.GetChild(0).transform.GetChild(1).GetComponent<Image>(); // change
        _isPlayerExist = true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Bullet")
        {
            _hp -= collider.gameObject.GetComponent<BaseBullet>().Power; // does it neet to create event?
            _healthBar.fillAmount = _hp / MaxHP;
        }
        if (_hp <= 0)
        {
            OnEnemyDied?.Invoke(this);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*if (collision.gameObject.tag == "Enemy")
        //need to fix collision between enemies
        */
    }

    private void FixedUpdate()
    {
        if (_isPlayerExist)
        {
            Vector3 positionPerFrame = _targetPosition * Time.fixedDeltaTime;
            transform.rotation = Quaternion.LookRotation(positionPerFrame);
            _rigidBody.MovePosition(transform.position + positionPerFrame);
        }
    }

    public void ToMove(Vector3 position) => _targetPosition = (position - transform.position) * Speed;

    public void ToStay()
    {
        _isPlayerExist = false;
        _animator.SetBool("isMoving", false);
    }
}
