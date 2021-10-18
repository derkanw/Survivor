using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BaseEnemy : MonoBehaviour
{
    public event Action<BaseEnemy, float> OnEnemyDied;

    [SerializeField] private Stat Health;
    [SerializeField] private Stat Rapidity;
    [SerializeField] private Stat Power;
    [SerializeField] private Stat DeathPoints;

    private Rigidbody _rigidBody;
    private Vector3 _targetPosition;
    private Animator _animator;
    private Image _healthBar;
    private float _hp;
    private bool _isPlayerExist;


    private void Start()
    {
        Health.Init();
        Rapidity.Init();
        Power.Init();
        DeathPoints.Init();

        _hp = Health.Value;
        _isPlayerExist = true;
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _animator = gameObject.GetComponent<Animator>();
        _healthBar = transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        var target = collider.gameObject;
        if (target.tag == "Player")
            target.GetComponent<Player>().TakeDamage(Power.Value);
        if (_hp <= 0)
        {
            OnEnemyDied?.Invoke(this, DeathPoints.Value);
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

    public void TakeDamage(float power)
    {
        _hp -= power;
        _healthBar.fillAmount = _hp / Health.Value;
    }

    public void OnLevelUp(int level)
    {
        Health.Modify(level);
        Rapidity.Modify(level);
        Power.Modify(level);
        DeathPoints.Modify(level);
    }

    public void ToMove(Vector3 position) => _targetPosition = (position - transform.position) * Rapidity.Value;

    public void ToStay()
    {
        _isPlayerExist = false;
        _animator.SetBool("isMoving", false);
    }
}
