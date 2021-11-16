using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public class Player : MonoBehaviour
{
    public event Action<float> ChangedHP;
    public event Action<float> ChangedMana;
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
    private float _hp;
    private float _mana;
    private WeaponsManager _weaponsManager;


    // TODO: auto replenishment of mana
    public bool SpendMana(float count)
    {
        if (_mana < count)
            return false;
        _mana -= count;
        ChangedMana?.Invoke(_mana / Mana.Value);
        return true;
    }

    public void TopUpMana(float incMana)
    {
        AudioManager.PlaySound(SoundNames.ManaSound);
        _mana += incMana;
        if (_mana > Mana.Value)
            _mana = Mana.Value;
        ChangedMana?.Invoke(_mana / Mana.Value);
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
        _weaponsManager.SetGunParams(Agility.Value, Power.Value);
    }

    public void MoveTo(Vector3 position) => _position = position * Rapidity.Value;

    public void LookTo(Vector3 direction) => transform.LookAt(direction);

    private void FixedUpdate()
    {
        if (_hp <= 0) return;
        if (_position == Vector3.zero)
            _animator.SetBool("isMoving", false);
        else
        {
            _animator.SetBool("isMoving", true);
            _rigidBody.MovePosition(transform.position + _position * Time.fixedDeltaTime);
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
        Mana.Init();
        Rapidity.Init();
        Agility.Init();
        Power.Init();

        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _animator = gameObject.GetComponent<Animator>();
        _hp = Health.Value;
        _weaponsManager = gameObject.GetComponent<WeaponsManager>();
    }
}
