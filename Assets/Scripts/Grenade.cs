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
            //var explosion = Instantiate(Explosion, transform.position, Quaternion.identity);
            if (Physics.SphereCast(transform.position, Radius, Vector3.forward, out RaycastHit hit, _mask))
            {
                print(1);
                hit.collider.gameObject.GetComponent<BaseEnemy>().TakeDamage(Power);

            }
            Destroy(gameObject);
        }
    }
}