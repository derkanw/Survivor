using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBullet : MonoBehaviour
{
    [SerializeField] [Range(0f, 50f)] protected float Power;
    [SerializeField] [Range(0f, 20f)] protected float Speed;

    public void SetPower(float incPower) => Power *= incPower;

    protected abstract void Move();

    private void FixedUpdate()
    {
        Move();
        if (transform.position.y <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        var target = collider.gameObject;
        if (target.tag == "Enemy")
        {
            target.GetComponent<BaseEnemy>().TakeDamage(Power);
            Destroy(gameObject);
        }
    }
}
