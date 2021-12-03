using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public class Player : MonoBehaviour
{
    public event Action<float> ChangedHP;
    public event Action<Vector3> Moved;
    public event Action Died;

    [SerializeField] private Stat Health;
    [SerializeField] private Stat Rapidity;
    [SerializeField] private Stat Agility;
    [SerializeField] private Stat Power;

    private Rigidbody _rigidBody;
    private Animator _animator;
    private Vector3 _position;
    private float _hp;
    private WeaponsManager _weaponsManager;
    private int _currentSpeedLevel;
    private int _speedLevel;
    private float _currentPower;

    public void PowerUp(float power, float time)
    {
        _weaponsManager.SetGunParams(Agility.Value, power);
        StartCoroutine(PowerUpEffect(time));
    }

    public void SpeedUp(int incSpeed, float time)
    {
        Rapidity.Modify(_speedLevel + incSpeed);
        StartCoroutine(SpeedUpEffect(time));
    }

    public void Heal(float incHP)
    {
        AudioManager.PlaySound(SoundNames.HealSound);
        _hp += incHP;
        if (_hp > Health.Value)
            _hp = Health.Value;
        ChangedHP?.Invoke(_hp / Health.Value);
    }

    public void TakeDamage(float power)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Death")) return;
        AudioManager.PlaySound(SoundNames.PlayerHit);
        _hp -= power;
        ChangedHP?.Invoke(_hp / Health.Value);
        _animator.SetTrigger("Damage");
        if (_hp <= 0)
            StartCoroutine(PlayerDied());
    }

    public void OnLevelUp(Dictionary<StatsNames, int> stats)
    {
        foreach (StatsNames name in stats.Keys)
            switch (name)
            {
                case StatsNames.Health:
                    Health.Modify(stats[name]);
                    break;
                case StatsNames.Rapidity:
                    _speedLevel = stats[name];
                    Rapidity.Modify(_speedLevel);
                    _currentSpeedLevel = _speedLevel;
                    break;
                case StatsNames.Agility:
                    Agility.Modify(stats[name]);
                    break;
                case StatsNames.Power:
                    Power.Modify(stats[name]);
                    break;
            }
        _weaponsManager.SetGunParams(Agility.Value, Power.Value);
    }

    public void MoveTo(Vector3 position) => _position = position * Rapidity.Value;

    public void LookTo(Vector3 direction) => transform.LookAt(direction);

    private IEnumerator PowerUpEffect(float time)
    {
        yield return new WaitForSeconds(time);
        _weaponsManager.SetGunParams(Agility.Value, Power.Value);
    }

    private IEnumerator SpeedUpEffect(float time)
    {
        yield return new WaitForSeconds(time);
        Rapidity.Modify(_speedLevel);
    }

    private void FixedUpdate()
    {
        if (_hp <= 0) return;
        if (_position == Vector3.zero)
            _animator.SetBool("isMoving", false);
        else
        {
            _animator.SetBool("isMoving", true);
            _rigidBody.MovePosition(transform.position + _position * Time.fixedDeltaTime);
            AudioManager.PlaySound(SoundNames.PlayerMove);
        }
        Moved?.Invoke(transform.position);
    }

    private IEnumerator PlayerDied()
    {
        AudioManager.PlaySound(SoundNames.PlayerDie);
        _animator.SetTrigger("Death");
        yield return new WaitForSeconds(2f);
        Died?.Invoke();
        Destroy(gameObject);
    }

    private void Awake()
    {
        Health.Init();
        Rapidity.Init();
        Agility.Init();
        Power.Init();

        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _animator = gameObject.GetComponent<Animator>();
        _hp = Health.Value;
        _weaponsManager = gameObject.GetComponent<WeaponsManager>();
    }
}
