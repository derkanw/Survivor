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
    private bool _isPlayerExists;
    private bool _isAttacking;
    private float _hp;
    private float _attackSpeed;

    public void OnLevelUp(int level)
    {
        Health.Modify(level);
        Rapidity.Modify(level);
        Power.Modify(level);
        DeathPoints.Modify(level);
        _hp = Health.Value;
    }

    public void MoveTo(Vector3 position)
    {
        _targetPosition = position - transform.position;
        if (_targetPosition.magnitude >= 1)
            _targetPosition = _targetPosition.normalized;
    }

    public void Stay()
    {
        _isPlayerExists = false;
        _animator.SetBool("isMoving", false);
    }

    public void TakeDamage(float power)
    {
        // TODO: when an enemy dies, sometimes it deals damage or the death animation doesn't play
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Death")) return;
        _hp -= power;
        HealthBar.fillAmount = _hp / Health.Value;
        _animator.SetTrigger("Damage");
        if (_hp <= 0)
            StartCoroutine(Died());
    }

    private void Start()
    {
        Health.Init();
        Rapidity.Init();
        Power.Init();
        DeathPoints.Init();

        _isPlayerExists = true;
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _animator = gameObject.GetComponent<Animator>();
        _hp = Health.Value;
        _attackSpeed = 3f;
    }

    private IEnumerator Died()
    {
        EnemyDied?.Invoke(this, DeathPoints.Value);
        _animator.SetTrigger("Death");
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }

    private IEnumerator Attack(GameObject target)
    {
        while (_isPlayerExists && _isAttacking)
        {
            _animator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.8f);
            if (target != null)
                target.GetComponent<Player>().TakeDamage(Power.Value);
            yield return new WaitForSeconds(_attackSpeed);
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        var target = collider.gameObject;
        if (target.CompareTag("Player") && !_isAttacking)
        {
            _isAttacking = true;
            _animator.SetBool("isMoving", false);
            StartCoroutine(Attack(target));
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        _isAttacking = false;
        _animator.SetBool("isMoving", true);
    }

    private void FixedUpdate()
    {
        if (_isPlayerExists)
        {
            Vector3 targetPos = _targetPosition * Rapidity.Value * Time.fixedDeltaTime;
            transform.rotation = Quaternion.LookRotation(targetPos);
            if (!_isAttacking)
                _rigidBody.MovePosition(transform.position + targetPos);
        }
    }
}
