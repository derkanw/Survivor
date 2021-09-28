using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineBullet : BaseBullet
{
    private Rigidbody _rigidBody;

    protected override void Flying()
    {
        _rigidBody.MovePosition(transform.position + Direction * Speed * Time.fixedDeltaTime);
    }

    public void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>(); 
    }
}
