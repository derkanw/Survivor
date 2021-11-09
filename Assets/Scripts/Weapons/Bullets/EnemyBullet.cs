using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MachineBullet
{
    private void OnTriggerEnter(Collider collider)
    {
        var target = collider.gameObject;
        if (target.CompareTag("Player"))
        {
            var player = target.GetComponent<Player>();
            player.TakeDamage(Power);
            Destroy(gameObject);
        }
    }
}
