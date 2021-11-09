using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBullet : MachineBullet
{
    [SerializeField] [Range(0f, 20f)] private float FreezeTime;
    [SerializeField] [Range(-10f, 0f)] private int FreezeIncSpeed;
    [SerializeField] private GameObject Effect;

    protected override void DamageEffect(BaseEnemy enemy)
    {
        Instantiate(Effect, transform.position, Quaternion.identity);
        StartCoroutine(enemy.DecreaseSpeed(FreezeIncSpeed, FreezeTime));
    }
}