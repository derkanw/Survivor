using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : BaseGun
{
    protected override void InitBullet()
    {
        GameObject bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
        bullet.transform.LookAt(_direction);
        bullet.GetComponent<BaseBullet>().SetPower(_incPower);
    }
}
