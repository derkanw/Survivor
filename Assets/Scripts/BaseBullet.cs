using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBullet : MonoBehaviour
{
    [Range(0f, 50f)] public float Power;
    [SerializeField] [Range(0f, 20f)] protected float Speed;

    protected abstract void Move ();

    private void FixedUpdate()
    {
        Move();
        if (transform.position.y <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
            Destroy(gameObject);
    }
}
 