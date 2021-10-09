using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseGun : MonoBehaviour
{
    public event Action<string> OnChangedBulletsCount;
    public event Action<float> OnReload;

    [SerializeField] protected GameObject BulletPrefab;
    protected Vector3 _direction;

    [SerializeField] [Range(0f, 50f)] private float ReloadTime;
    [SerializeField] [Range(0f, 20f)] private float ShootingSpeed;
    [SerializeField][Range(0f, 100f)] private float ClipSize;

    private bool _isReloading; // so much
    private bool _isShooting;
    private bool _isMouseDown;
    private bool _isReloadingKeyDown;
    private float _bulletsCount;

    protected abstract void InitBullet();

    private IEnumerator Shoot()
    {
        _isShooting = true;
        while (_bulletsCount != 0 && _isMouseDown)
        {
            InitBullet();
            --_bulletsCount;
            OnChangedBulletsCount?.Invoke(_bulletsCount + "\\" + ClipSize);
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
        OnChangedBulletsCount?.Invoke(_bulletsCount + "\\" + ClipSize);
    }

    private void Start()
    {
        _bulletsCount = ClipSize;
        _isReloading = false;
        _isShooting = false;
    }

    private void Update()
    {
        if (_isReloadingKeyDown && !_isReloading)
        {
            _isShooting = true;
            StartCoroutine(Reload());
            OnReload?.Invoke(0f);
        }
        if (_isReloading)
            OnReload?.Invoke(Time.deltaTime / ReloadTime);
        if (_isMouseDown && !_isShooting)
        {
            StartCoroutine(Shoot());
            _isShooting = false;
        }
    }

    public void ToMouseDown(bool value) => _isMouseDown = value;

    public void ToReloadingKeyDown(bool value) => _isReloadingKeyDown = value;

    public void ToLook(Vector3 direction) => _direction = direction;
}
