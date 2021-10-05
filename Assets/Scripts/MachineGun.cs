using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : BaseGun
{
    protected override void InitBullet()
    {
        Transform root = transform.root;
        GameObject bullet = Instantiate(BulletPrefab, transform.position, root.rotation * Quaternion.Euler(90f, 0f, 0f));
        bullet.GetComponent<BaseBullet>().direction = root.GetComponent<Player>().direction;
    }
}
