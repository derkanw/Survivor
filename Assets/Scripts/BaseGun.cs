using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class BaseGun : MonoBehaviour
{
    public event Action<float> ChangedBulletsCount;
    public event Action<float> ChangedClipSize;
    public event Action<float> Reloading;

    [SerializeField] protected GameObject BulletPrefab;
    protected Vector3 _direction;
    protected float _incPower;

    [SerializeField] [Range(0f, 50f)] private float ReloadTime;
    [SerializeField] [Range(0f, 20f)] private float ShootingSpeed;
    [SerializeField][Range(0f, 100f)] private float ClipSize;

    private bool _isReloading;
    private bool _isShooting;
    private bool _isMouseDown;
    private bool _isReloadingKeyDown;
    private float _bulletsCount;

    public void ToMouseDown(bool value) => _isMouseDown = value;

    public void ToReloadingKeyDown(bool value) => _isReloadingKeyDown = value;

    public void LookTo(Vector3 direction) => _direction = direction;

    public void SetParams(float incAgility, float incPower)
    {
        _incPower = incPower;
        ReloadTime *= incAgility;
        ShootingSpeed *= incAgility;
    }

    protected abstract void InitBullet();

    private IEnumerator Shoot()
    {
        _isShooting = true;
        while (_bulletsCount != 0 && _isMouseDown)
        {
            InitBullet();
            --_bulletsCount;
            yield return new WaitForSeconds(ShootingSpeed);
        }
        _isReloading = false;
    }

    private IEnumerator Reload()
    {
        _isReloading = true;
        yield return new WaitForSeconds(ReloadTime);
        _bulletsCount = ClipSize;
        _isReloading = false;
        _isShooting = false;
    }

    private void Start()
    {
        _bulletsCount = ClipSize;
        _isReloading = false;
        _isShooting = false;
        _incPower = 1;
        ChangedClipSize?.Invoke(ClipSize);
    }

    private void Update()
    {
        // reloading image is filled wrong
        if (_isReloadingKeyDown && !_isReloading)
        {
            _isShooting = true;
            StartCoroutine(Reload());
            Reloading?.Invoke(0f);
        }
        if (_isReloading)
            Reloading?.Invoke(Time.deltaTime / ReloadTime);
        if (_isMouseDown && !_isShooting)
        {
            StartCoroutine(Shoot());
            _isShooting = false;
        }
        ChangedBulletsCount?.Invoke(_bulletsCount);
    }
}
