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

    public void MoveTo(Vector3 position) => _targetPosition = (position - transform.position) * Rapidity.Value;

    public void Stay()
    {
        _isPlayerExist = false;
        _animator.SetBool("isMoving", false);
    }

    public void TakeDamage(float power)
    {
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

        _isPlayerExist = true;
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
        while (_isAttacking)
        {
            if (target == null || _animator.GetCurrentAnimatorStateInfo(0).IsName("Death")) yield break;
            _animator.SetTrigger("Attack");
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
            StartCoroutine(Attack(target));
        }
    }

    private void OnTriggerExit(Collider collider) => _isAttacking = false;

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
