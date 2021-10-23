using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineBullet : BaseBullet
{
    private Rigidbody _rigidBody;
    private Vector3 _offset;

    protected override void Move()
    {
        _rigidBody.AddForce(transform.forward * Speed);
    }

    private void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _offset = transform.position;
    }
}
