using UnityEngine;

public class FlameGun : BaseGun
{
    protected override void InitBullet()
    {
        AudioManager.PlaySound(SoundNames.FlameGun);
        GameObject bullet = Instantiate(BulletPrefab, _offset.position, Quaternion.identity);
        bullet.transform.LookAt(_direction);
        bullet.GetComponent<BaseBullet>().SetPower(_incPower);
    }
}
