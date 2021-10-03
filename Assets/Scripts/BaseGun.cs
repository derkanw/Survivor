using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGun : MonoBehaviour
{
    public GameObject BulletPrefab;
    [Range(0f, 50f)]
    public float ReloadTime, ShootingSpeed, ClipSize;
    protected bool isReloading, isShooting;
    private float _bulletsCount;

    protected abstract void InitBullet();

    protected IEnumerator Shooting()
    {
        isShooting = true;
        while (_bulletsCount != 0 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            InitBullet();
            --_bulletsCount;
            yield return new WaitForSeconds(ShootingSpeed);
        }
    }

    protected IEnumerator Reloading()
    {
        isReloading = true;
        yield return new WaitForSeconds(ReloadTime);
        _bulletsCount = ClipSize;
    }

    public void Start()
    {
        _bulletsCount = ClipSize;
        isReloading = false;
        isShooting = false;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            StartCoroutine(Reloading());
            isReloading = false;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isShooting)
        {
            StartCoroutine(Shooting());
            isShooting = false;
        }
    }
}
