using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGun : MonoBehaviour
{
    [SerializeField] protected GameObject BulletPrefab;
    [SerializeField] [Range(0f, 50f)] private float ReloadTime;
    [SerializeField] [Range(0f, 20f)] private float ShootingSpeed;
    [SerializeField][Range(0f, 100f)] private float ClipSize;

    private bool _isReloading;
    private bool _isShooting;
    private float _bulletsCount;

    protected abstract void InitBullet();

    private IEnumerator Shooting()
    {
        _isShooting = true;
        while (_bulletsCount != 0 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            InitBullet();
            --_bulletsCount;
            yield return new WaitForSeconds(ShootingSpeed);
        }
    }

    private IEnumerator Reloading()
    {
        _isReloading = true;
        yield return new WaitForSeconds(ReloadTime);
        _bulletsCount = ClipSize;
    }

    private void Start()
    {
        _bulletsCount = ClipSize;
        _isReloading = false;
        _isShooting = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !_isReloading)
        {
            StartCoroutine(Reloading());
            _isReloading = false;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && !_isShooting)
        {
            StartCoroutine(Shooting());
            _isShooting = false;
        }
    }
}
