using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBullet : MonoBehaviour
{
    [Range(0f, 50f)] public float Power;
    [HideInInspector] public Vector3 direction;
    [SerializeField] [Range(0f, 20f)] protected float Speed;

    protected abstract void Movement();

    private void FixedUpdate()
    {
        Movement();
        if (transform.position.y <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
            Destroy(gameObject);
    }
}
