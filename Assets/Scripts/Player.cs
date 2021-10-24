using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public event Action<float> ChangedHP;
    public event Action<Vector3> Moved;
    public event Action Died;

    [SerializeField] private Stat Health;
    [SerializeField] private Stat Mana;
    [SerializeField] private Stat Rapidity;
    [SerializeField] private Stat Agility;
    [SerializeField] private Stat Power;

    private Rigidbody _rigidBody;
    private Animator _animator;
    private Vector3 _position;
    private BaseGun _gun;
    private float _hp;

    public void TakeDamage(float power)
    {
        _hp -= power;
        ChangedHP?.Invoke(_hp / Health.Value);
    }

    public void OnLevelUp(Dictionary<StatsNames, int> stats)
    {
        foreach (StatsNames name in stats.Keys)
            switch (name)
            {
                case StatsNames.Health:
                    Health.Modify(stats[name]);
                    break;
                case StatsNames.Mana:
                    Mana.Modify(stats[name]);
                    break;
                case StatsNames.Rapidity:
                    Rapidity.Modify(stats[name]);
                    break;
                case StatsNames.Agility:
                    Agility.Modify(stats[name]);
                    break;
                case StatsNames.Power:
                    Power.Modify(stats[name]);
                    break;
            }
        _gun.SetParams(Agility.Value, Power.Value);
    }

    public void MoveTo(Vector3 position) => _position = position * Rapidity.Value;

    public void LookTo(Vector3 direction) => transform.LookAt(direction);

    private void FixedUpdate()
    {
        if (_position == Vector3.zero)
            _animator.SetBool("isMoving", false);
        else
        {
            _animator.SetBool("isMoving", true);
            _rigidBody.MovePosition(transform.position + _position * Time.fixedDeltaTime);
        }
        Moved?.Invoke(transform.position);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (_hp <= 0)
        {
            Died?.Invoke();
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        Health.Init();
        Mana.Init();
        Rapidity.Init();
        Agility.Init();
        Power.Init();

        _gun = GameObject.FindWithTag("Gun").GetComponent<BaseGun>(); // how it will be changed if there is a list of weapons?
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _animator = gameObject.GetComponent<Animator>();
        _hp = Health.Value;
    }
}
