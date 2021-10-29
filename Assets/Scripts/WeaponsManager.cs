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
    public event Action<int> GetWeaponsCount;
    public event Action<float> ChangedBulletsCount;
    public event Action<float> ChangedClipSize;
    public event Action<float> Reloading;

    [SerializeField] private Transform RightGunBone;
    //[SerializeField] private Transform LeftGunBone;
    [SerializeField] private Arsenal[] Arsenal;

    private List<BaseGun> _guns;
    private int _currentGun;
    private Animator _animator;
    private int _gunsCount;
    private int _skillsCount;

    public void LookTo(Vector3 direction) => _guns[_currentGun].LookTo(direction);
    public void SetShooting(bool value) => _guns[_currentGun].SetShooting(value);
    public void SetReloading(bool value) => _guns[_currentGun].SetReloading(value);

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
        _gunsCount = Arsenal.Length;
        _guns = new List<BaseGun>(_gunsCount);
        _animator = gameObject.GetComponent<Animator>();
        InitArsenal();
    }

    private void Start()
    {
        _currentGun = 0;
        if (_gunsCount > 0)
            SetArsenal(_currentGun);
        GetWeaponsCount?.Invoke(_gunsCount + _skillsCount);
    }

    private void InitArsenal()
    {
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
            }
            /*if (hand.leftGun != null)
            {
                GameObject newLeftGun = (GameObject)Instantiate(hand.leftGun);
                newLeftGun.transform.parent = leftGunBone;
                newLeftGun.transform.localPosition = Vector3.zero;
                newLeftGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
            }*/
        }
    }

    public void SetArsenal(int index)
    {
        //print(index);
        //print(_guns.Count);
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
    }
}
