using System.Collections;
using UnityEngine;
using System;

public abstract class BaseGun : MonoBehaviour, IGun
{
    public event Action<float> Reloading;
    public event Action Shooting;

    [SerializeField] protected GameObject BulletPrefab;
    protected Vector3 _direction;
    protected float _incPower;
    protected Transform _offset;

    [SerializeField] [Range(0f, 50f)] private float ReloadTime;
    [SerializeField] [Range(0f, 20f)] private float ShootingSpeed;
    [SerializeField] [Range(0f, 100f)] private float ClipSize;

    private bool _isReloading;
    private bool _isShooting;
    private bool _isCursorClicked;
    private bool _isReloadingClicked;
    private float _bulletCount;
    private float _reloadProgress;
    private float _timeOffset;

    public void SetShooting(bool value) => _isCursorClicked = value;
    public void SetReloading(bool value) => _isReloadingClicked = value;
    public float GetBulletCount() => _bulletCount;
    public float GetClipSize() => ClipSize;
    
    public void LookTo(Vector3 direction) => _direction = direction;

    public void SetParams(float incAgility, float incPower)
    {
        _incPower = incPower;
        ReloadTime *= incAgility;
        ShootingSpeed *= incAgility;
    }

    public void StopReloading()
    {
        StopAllCoroutines();
        Clear();
    }

    protected abstract void InitBullet();

    private IEnumerator Shoot()
    {
        _isShooting = true;
        while (_bulletCount != 0 && _isCursorClicked)
        {
            Shooting?.Invoke();
            yield return new WaitForSeconds(_timeOffset);
            InitBullet();
            --_bulletCount;
            yield return new WaitForSeconds(ShootingSpeed);
        }
        _isShooting = false;
    }
    private IEnumerator Reload()
    {
        _isShooting = true;
        _isReloading = true;
        while (_reloadProgress < 1f)
            yield return new WaitForEndOfFrame();
        _bulletCount = ClipSize;
        _isReloading = false;
        _isShooting = false;
    }

    private void Start()
    {
        Clear();
        _bulletCount = ClipSize;
        _incPower = 1;
        _offset = gameObject.transform.GetChild(0);
        _timeOffset = 0.5f;
    }

    private void Clear()
    {
        _isReloading = false;
        _isShooting = false;
        _reloadProgress = 1f;
    }

    private void Update()
    {
        if (!_isReloading && _isReloadingClicked)
        {
            _reloadProgress = 0f;
            Reloading?.Invoke(_reloadProgress);
            StartCoroutine(Reload());
            AudioManager.PlaySound(SoundNames.Reload);
        }
        if (_isReloading)
        {
            _reloadProgress += Time.deltaTime / ReloadTime;
            Reloading?.Invoke(_reloadProgress);
        }
        if (!_isShooting && _isCursorClicked)
            StartCoroutine(Shoot());
    }
}
