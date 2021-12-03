using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponsManager : MonoBehaviour
{
    public event Action<int> GetArsenalSize;
    public event Action<int> GetWeaponsCount;
    public event Action<float> ChangedBulletsCount;
    public event Action<float> ChangedClipSize;
    public event Action<float> Reloading;

    private event Action ChangedWeapon;

    [SerializeField] private Transform RightGunBone;
    [SerializeField] private Arsenal[] Arsenal;

    private List<BaseGun> _guns;
    private int _currentGun;
    private Animator _animator;
    private int _gunsCount;

    public void SetNewWeapon()
    {
        ++_gunsCount;
        SaveSystem.Save<int>(Tokens.Weapons, _gunsCount);
    }

    public void LookTo(Vector3 direction) => _guns[_currentGun].LookTo(direction);
    public void SetShooting(bool value) => _guns[_currentGun].SetShooting(value);
    public void SetReloading(bool value) => _guns[_currentGun].SetReloading(value);

    public void SetGunParams(float agility, float power)
    {
        foreach (BaseGun gun in _guns)
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
                _guns[i].gameObject.SetActive(true);
                _currentGun = index;
            }
            else
                _guns[i].gameObject.SetActive(false);
        }
        _animator.runtimeAnimatorController = Arsenal[index].Controller;
        var gun = _guns[_currentGun];
        ChangedClipSize?.Invoke(gun.GetClipSize());
        ChangedWeapon?.Invoke();
        Reloading?.Invoke(1f);
    }

    private void Awake()
    {
        _gunsCount = SaveSystem.IsExists(Tokens.Weapons) ? SaveSystem.Load<int>(Tokens.Weapons) : 1;
        _guns = new List<BaseGun>(_gunsCount);
        _animator = gameObject.GetComponent<Animator>();
        InitArsenal();
    }

    private void Start()
    {
        _currentGun = 0;
        if (_gunsCount > 0)
            SetArsenal(_currentGun);
        GetWeaponsCount?.Invoke(_gunsCount);
        GetArsenalSize?.Invoke(Arsenal.Length);
    }

    private void Update() => ChangedBulletsCount?.Invoke(_guns[_currentGun].GetBulletsCount());

    private void InitArsenal()
    {
        for (int index = 0; index < _gunsCount; ++index)
        {
            if (Arsenal[index].RightGun != null)
            {
                GameObject newRightGun = Instantiate(Arsenal[index].RightGun);
                newRightGun.transform.parent = RightGunBone;
                newRightGun.transform.localPosition = Vector3.zero;
                newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                newRightGun.SetActive(false);
                BaseGun gun = newRightGun.GetComponent<BaseGun>();
                gun.Reloading += OnReloading;
                gun.Shooting += OnShooting;
                ChangedWeapon += gun.StopReloading;
                _guns.Add(gun);
            }
        }
    }

    private void OnShooting() => _animator.SetTrigger("Attack");

    private void OnReloading(float count) => Reloading?.Invoke(count);

    private void OnDisable()
    {
        foreach (BaseGun gun in _guns)
        {
            gun.Reloading -= OnReloading;
            gun.Shooting -= OnShooting;
            ChangedWeapon -= gun.StopReloading;
        }
    }
}
