using UnityEngine;

public class MachineBullet : BaseBullet
{
    [SerializeField] [Range(0f, 20f)] protected float Speed;

    private Rigidbody _rigidBody;
    protected override void Move() => _rigidBody.AddForce(transform.forward * Speed);

    private void Start()
    {
        AudioManager.PlaySound(SoundNames.Rifle);
        _rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        var target = collider.gameObject;
        if (target.CompareTag("Enemy"))
        {
            var enemy = target.GetComponent<BaseEnemy>();
            enemy.TakeDamage(Power);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Move();
        if (transform.position.y <= 0)
            Destroy(gameObject);
    }
}
