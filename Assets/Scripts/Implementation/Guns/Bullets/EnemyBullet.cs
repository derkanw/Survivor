using UnityEngine;

public class EnemyBullet : MachineBullet
{
    private void Awake() => AudioManager.PlaySound(SoundNames.EnemyShoot);
    private void OnTriggerEnter(Collider collider)
    {
        var target = collider.gameObject;
        if (target.CompareTag("Player"))
        {
            var player = target.GetComponent<IPlayer>();
            player.TakeDamage(Power);
            Destroy(gameObject);
        }
    }
}
