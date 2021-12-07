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
    private IGunService _gunService;
    private int _speedLevel;

    public void SaveParams() => SaveSystem.Save<float>(Tokens.HP, _hp);

    public void PowerUp(float power, float time)
    {
        _gunService.SetGunParams(Agility.Value, power);
        StartCoroutine(PowerUpEffect(time));
    }

    public void SpeedUp(int incSpeed, float time)
    {
        Rapidity.Modify(_speedLevel + incSpeed);
        StartCoroutine(SpeedUpEffect(time));
    }

    public void Heal(float incHP)
    {
        _hp += incHP;
        if (_hp > Health.Value)
            _hp = Health.Value;
        ChangedHP?.Invoke(_hp / Health.Value);
    }

    public void TakeDamage(float power)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Death")) return;
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
                    break;
                case StatsNames.Agility:
                    Agility.Modify(stats[name]);
                    break;
                case StatsNames.Power:
                    Power.Modify(stats[name]);
                    break;
            }
        _gunService.SetGunParams(Agility.Value, Power.Value);
    }

    public void MoveTo(Vector3 position) => _position = position * Rapidity.Value;

    public void LookTo(Vector3 direction) => transform.LookAt(direction);

    private IEnumerator PowerUpEffect(float time)
    {
        yield return new WaitForSeconds(time);
        _gunService.SetGunParams(Agility.Value, Power.Value);
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
        _hp = SaveSystem.IsExists(Tokens.HP) ? SaveSystem.Load<float>(Tokens.HP) : Health.Value;
        _gunService = gameObject.GetComponent<IGunService>();
    }

    private void Start() => ChangedHP?.Invoke(_hp / Health.Value);
}
