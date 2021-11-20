using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class GrenadeLauncher : BaseGun
{
    // bad idea

    [SerializeField] private GameObject WeaponEffect;
    [SerializeField] [Range(0f, 10f)] private float ManaCost;
    public event Func<float, bool> CanShoot;
    public event Action LackMana;

    protected override void InitBullet()
    {
        if (CanShoot(ManaCost)?.Invoke())
            Instantiate(WeaponEffect, _offset.position, transform.rotation * Quaternion.Euler(90f, 0f, 90f));
        else
            LackMana?.Invoke();
        GameObject bullet = Instantiate(BulletPrefab, _offset.position, Quaternion.identity);
        bullet.transform.LookAt(_direction);
        bullet.GetComponent<BaseBullet>().SetPower(_incPower);
    }

}*/
