using UnityEngine;

public class MachineGun : BaseGun
{
    [SerializeField] private GameObject WeaponEffect;
    protected override void InitBullet()
    {
        Instantiate(WeaponEffect, _offset.position, transform.rotation * Quaternion.Euler(90f, 0f, 90f));
        GameObject bullet = Instantiate(BulletPrefab, _offset.position, Quaternion.identity);
        bullet.transform.LookAt(_direction);
        bullet.GetComponent<BaseBullet>().SetPower(_incPower);
    }
}
