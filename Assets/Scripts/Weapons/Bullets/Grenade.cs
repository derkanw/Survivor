using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : BaseBullet
{
    [SerializeField] [Range(0f, 20f)] private float Speed;
    [SerializeField] [Range(0f, 20f)] private float Radius;
    [SerializeField] private GameObject Explosion;
    [SerializeField] private GameObject FireTail;
    [SerializeField] private Transform TailPosition;

    private Rigidbody _rigidBody;
    private int _mask;
    protected override void Move() => _rigidBody.AddForce(transform.forward * Speed);
    private void Start()
    {
        _mask = 1 << 3;
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        var _fireTail = Instantiate(FireTail, TailPosition.position, transform.rotation * Quaternion.Euler(0f, 90f, 0f));
        _fireTail.transform.SetParent(TailPosition);
    }

    private void FixedUpdate()
    {
        Move();
        if (transform.position.y <= 0)
        {
            Instantiate(Explosion, transform.position, Quaternion.identity);
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, Radius, _mask);
            foreach (var hitCollider in hitColliders)
                hitCollider.gameObject.GetComponent<BaseEnemy>().TakeDamage(Power);
            Destroy(gameObject);
        }
    }
}