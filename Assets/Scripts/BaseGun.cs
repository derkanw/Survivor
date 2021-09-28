using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGun : MonoBehaviour
{
    public GameObject BulletPrefab;
    [Range(0f, 50f)]
    public float ReloadTime, ShootingSpeed, ClipSize;

    protected float _bulletsCount;

    protected abstract void InitBullet();

    protected IEnumerator Shooting()
    {
        while (_bulletsCount != 0 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            InitBullet();
            --_bulletsCount;
            yield return new WaitForSeconds(ShootingSpeed);
        }
    }

    protected IEnumerator Reloading()
    {
        yield return new WaitForSeconds(ReloadTime);
        _bulletsCount = ClipSize;
    }

    public void Start()
    {
        _bulletsCount = ClipSize;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            StartCoroutine(Reloading());
        if (Input.GetKeyDown(KeyCode.Mouse0))
            StartCoroutine(Shooting());
    }
}
