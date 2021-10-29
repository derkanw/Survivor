using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BaseEnemy : MonoBehaviour
{
    public event Action<BaseEnemy, float> EnemyDied;

    [SerializeField] private Image HealthBar;
    [SerializeField] private Stat Health;
    [SerializeField] private Stat Rapidity;
    [SerializeField] private Stat Power;
    [SerializeField] private Stat DeathPoints;

    private Rigidbody _rigidBody;
    private Vector3 _targetPosition;
    private Animator _animator;
    private bool _isPlayerExist;
    private float _hp;

    public void OnLevelUp(int level)
    {
        Health.Modify(level);
        Rapidity.Modify(level);
        Power.Modify(level);
        DeathPoints.Modify(level);
        _hp = Health.Value;
    }

    public void MoveTo(Vector3 position) => _targetPosition = (position - transform.position) * Rapidity.Value;

    public void Stay()
    {
        _isPlayerExist = false;
        _animator.SetBool("isMoving", false);
    }

    public void TakeDamage(float power)
    {
        _hp -= power;
        HealthBar.fillAmount = _hp / Health.Value;
    }

    private void Start()
    {
        Health.Init();
        Rapidity.Init();
        Power.Init();
        DeathPoints.Init();

        _isPlayerExist = true;
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _animator = gameObject.GetComponent<Animator>();
        _hp = Health.Value;
    }

    private void OnTriggerEnter(Collider collider)
    {
        var target = collider.gameObject;
        if (target.CompareTag("Player"))
            target.GetComponent<Player>().TakeDamage(Power.Value);
        if (_hp <= 0)
        {
            EnemyDied?.Invoke(this, DeathPoints.Value);
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
            Vector3 targetPos = _targetPosition * Time.fixedDeltaTime;
            transform.rotation = Quaternion.LookRotation(targetPos);
            _rigidBody.MovePosition(transform.position + targetPos);
        }
    }
}
