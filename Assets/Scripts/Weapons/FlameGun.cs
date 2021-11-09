using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameGun : BaseGun
{
    protected override void InitBullet()
    {
        GameObject bullet = Instantiate(BulletPrefab, _offset.position, Quaternion.identity);
        bullet.transform.LookAt(_direction);
        bullet.GetComponent<BaseBullet>().SetPower(_incPower);
    }
}
