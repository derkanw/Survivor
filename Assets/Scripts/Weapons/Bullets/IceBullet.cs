using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBullet : MachineBullet
{
    [SerializeField] [Range(0f, 20f)] private float FreezeTime;
    [SerializeField] [Range(-10f, 0f)] private int FreezeIncSpeed;
    [SerializeField] private GameObject Effect;

    private void OnTriggerEnter(Collider collider)
    {
        var target = collider.gameObject;
        if (target.CompareTag("Enemy"))
        {
            var enemy = target.GetComponent<BaseEnemy>();
            enemy.TakeDamage(Power);
            Instantiate(Effect, transform.position, Quaternion.identity);
            StartCoroutine(enemy.DecreaseSpeed(FreezeIncSpeed, FreezeTime));
            Destroy(gameObject);
        }
    }
}