using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public struct Arsenal
{
    public string Name;
    public GameObject RightGun;
    //public GameObject LeftGun;
    public RuntimeAnimatorController Controller;
}

public class WeaponsManager : MonoBehaviour
{
    public event Action<List<string>> GetWeapons;
    public event Action<int> ChangedWeapon;

    public event Action<float> ChangedBulletsCount;
    public event Action<float> ChangedClipSize;
    public event Action<float> Reloading;

    [SerializeField] private Transform RightGunBone;
    //[SerializeField] private Transform LeftGunBone;
    [SerializeField] private Arsenal[] Arsenal;

    private List<BaseGun> _guns;
    private int _currentGun;
    private Animator _animator;

    public void LookTo(Vector3 direction) => _guns[_currentGun].LookTo(direction);
    public void OnMouseDown(bool value) => _guns[_currentGun].OnMouseDown(value);
    public void OnReloadingKeyDown(bool value) => _guns[_currentGun].OnReloadingKeyDown(value);

    private void OnChangedBulletCount(float count) => ChangedBulletsCount?.Invoke(count);
    private void OnChangedClipSize(float count) => ChangedClipSize?.Invoke(count);
    private void OnReloading(float count) => Reloading?.Invoke(count);

    private void OnShooting() => _animator.SetTrigger("Attack");

    public void SetGunParams(float agility, float power)
    {
        foreach (BaseGun gun in _guns)
            gun.SetParams(agility, power);
    }

    private void Awake()
    {
        _guns = new List<BaseGun>(Arsenal.Length);
        _animator = gameObject.GetComponent<Animator>();
        InitArsenal();
        _currentGun = 0;
        if (Arsenal.Length > 0)
            SetArsenal(_currentGun);
    }

    private void InitArsenal()
    {
        List<string> weaponsNames = new List<string>(Arsenal.Length);
        foreach (Arsenal hand in Arsenal)
        {
            if (hand.RightGun != null)
            {
                GameObject newRightGun = Instantiate(hand.RightGun);
                newRightGun.transform.parent = RightGunBone;
                newRightGun.transform.localPosition = Vector3.zero;
                newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                newRightGun.SetActive(false);
                BaseGun gun = newRightGun.GetComponent<BaseGun>();
                gun.ChangedBulletsCount += OnChangedBulletCount;
                gun.ChangedClipSize += OnChangedClipSize;
                gun.Reloading += OnReloading;
                gun.Shooting += OnShooting;
                _guns.Add(gun);
                weaponsNames.Add(hand.Name);
            }
            /*if (hand.leftGun != null)
            {
                GameObject newLeftGun = (GameObject)Instantiate(hand.leftGun);
                newLeftGun.transform.parent = leftGunBone;
                newLeftGun.transform.localPosition = Vector3.zero;
                newLeftGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
            }*/
        }
        GetWeapons?.Invoke(weaponsNames);
    }

    private void SetArsenal(int index)
    {
        for (int i = 0; i < _guns.Count; ++i)
        {
            if (i == index)
            {
                _guns[i].gameObject.SetActive(true);
                _currentGun = index;
            }
            else
                _guns[i].gameObject.SetActive(false);
        }
        _animator.runtimeAnimatorController = Arsenal[index].Controller;
        ChangedWeapon?.Invoke(index);
    }
}
