using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : BaseGun
{
    protected override void InitBullet()
    {
        GameObject bullet = Instantiate(BulletPrefab, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z),
            transform.rotation * Quaternion.Euler(90f, 0f, 0f));
        bullet.GetComponent<BaseBullet>().Direction = gameObject.GetComponent<PlayerMovement>().direction;
    }
}
