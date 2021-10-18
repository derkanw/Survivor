using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBullet : MonoBehaviour
{
<<<<<<< Updated upstream
    [Range(0f, 50f)] public float Power;
    [HideInInspector] public Vector3 direction;
=======
    [SerializeField] [Range(0f, 50f)] protected float Power;
>>>>>>> Stashed changes
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
        var target = collider.gameObject;
        if (target.tag == "Enemy")
        {
            target.GetComponent<BaseEnemy>().TakeDamage(Power);
            Destroy(gameObject);
        }
    }

    public void SetPower(float incPower) => Power *= incPower;
}
