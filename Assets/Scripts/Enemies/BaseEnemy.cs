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
    [SerializeField] protected Stat Rapidity;
    [SerializeField] protected Stat Power;
    [SerializeField] private Stat DeathPoints;

    protected bool _isPlayerExists;
    protected bool _isAttacking;
    protected Animator _animator;
    protected Rigidbody _rigidBody;
    protected Vector3 _targetPosition;
    protected Vector3 _playerPosition;

    private float _hp;
    private int _level;

    public IEnumerator DecreaseSpeed(int inc, float time)
    {
        Rapidity.Modify(inc);
        while (Rapidity.Value < 0)
        {
            inc %= 2;
            Rapidity.Modify(inc);
        }
        yield return new WaitForSeconds(time);
        Rapidity.Modify(_level);
    }

    public void OnLevelUp(int level)
    {
        Health.Modify(level);
        Rapidity.Modify(level);
        Power.Modify(level);
        DeathPoints.Modify(level);
        _hp = Health.Value;
        _level = level;
    }

    public void MoveTo(Vector3 position)
    {
        _playerPosition = position;
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
        AudioManager.PlaySound(SoundNames.EnemyHit);
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
    }

    protected IEnumerator Died()
    {
        AudioManager.PlaySound(SoundNames.EnemyDie);
        EnemyDied?.Invoke(this, DeathPoints.Value);
        _animator.SetTrigger("Death");
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
}
