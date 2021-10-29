using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : BaseGun
{
    [SerializeField] private GameObject WeaponEffect;
    protected override void InitBullet()
    {
        Instantiate(WeaponEffect, _offset.position, Quaternion.identity);
        GameObject bullet = Instantiate(BulletPrefab, _offset.position, Quaternion.identity);
        bullet.transform.LookAt(_direction);
        bullet.GetComponent<BaseBullet>().SetPower(_incPower);
    }
}
