using System.Collections.Generic;
using UnityEngine;
using System;

public class GunService : MonoBehaviour, IGunService
{
    public event Action<int> GetArsenalSize;
    public event Action<int> GetGunCount;
    public event Action<float> ChangedBulletCount;
    public event Action<float> ChangedClipSize;
    public event Action<float> Reloading;

    private event Action ChangedWeapon;

    [SerializeField] private Transform RightGunBone;
    [SerializeField] private Arsenal[] Arsenal;

    private List<IGun> _guns;
    private int _currentGun;
    private Animator _animator;
    private int _gunCount;

    public void SetNewGun()
    {
        ++_gunCount;
        SaveSystem.Save<int>(Tokens.Guns, _gunCount);
    }

    public void LookTo(Vector3 direction) => _guns[_currentGun].LookTo(direction);
    public void SetShooting(bool value) => _guns[_currentGun].SetShooting(value);
    public void SetReloading(bool value) => _guns[_currentGun].SetReloading(value);

    public void SetGunParams(float agility, float power)
    {
        foreach (IGun gun in _guns)
            gun.SetParams(agility, power);
    }

    public void SetArsenal(int index)
    {
        if (_guns[index] == null)
            return;
        AudioManager.PlaySound(SoundNames.Equip);
        for (int i = 0; i < _guns.Count; ++i)
        {
            if (_guns[i] == null)
                continue;
            if (i == index)
            {
                _currentGun = index;
                break;
            }
        }
        _animator.runtimeAnimatorController = Arsenal[index].Controller;
        var gun = _guns[_currentGun];
        ChangedClipSize?.Invoke(gun.GetClipSize());
        ChangedWeapon?.Invoke();
        Reloading?.Invoke(1f);
    }

    private void Awake()
    {
        _gunCount = SaveSystem.IsExists(Tokens.Guns) ? SaveSystem.Load<int>(Tokens.Guns) : 1;
        _guns = new List<IGun>(_gunCount);
        _animator = gameObject.GetComponent<Animator>();
        InitArsenal();
    }

    private void Start()
    {
        _currentGun = 0;
        if (_gunCount > 0)
            SetArsenal(_currentGun);
        GetGunCount?.Invoke(_gunCount);
        GetArsenalSize?.Invoke(Arsenal.Length);
    }

    private void Update() => ChangedBulletCount?.Invoke(_guns[_currentGun].GetBulletCount());

    private void InitArsenal()
    {
        for (int index = 0; index < _gunCount; ++index)
        {
            if (Arsenal[index].RightGun != null)
            {
                GameObject newRightGun = Instantiate(Arsenal[index].RightGun);
                newRightGun.transform.parent = RightGunBone;
                newRightGun.transform.localPosition = Vector3.zero;
                newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                IGun gun = newRightGun.GetComponent<IGun>();
                gun.Reloading += OnReloading;
                gun.Shooting += OnShooting;
                ChangedWeapon += gun.StopReloading;
                _guns.Add(gun);
            }
        }
    }

    private void OnShooting() => _animator.SetTrigger("Attack");

    private void OnReloading(float count) => Reloading?.Invoke(count);
}
