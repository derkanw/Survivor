using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [Range(0f, 10f)] public float Speed;

    [Range(0f, 100f)] public float HP, Power;

    private GameObject _player;
    private Rigidbody _rigidBody;
    private Vector3 _position, _offset;
    private Animator _animator;

    public void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _player = GameObject.FindWithTag("Player");
        _animator = gameObject.GetComponent<Animator>();
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Bullet")
            HP -= collider.gameObject.GetComponent<BaseBullet>().Power;
        if (HP == 0)
            Destroy(gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {
        /*if (collision.gameObject.tag == "Enemy")
        //need to fix collision between enemies
        */
    }

    public void FixedUpdate()
    {
        if (_player != null)
        {
            Vector3 targetPos = (_player.transform.position - transform.position) * Speed * Time.fixedDeltaTime;
            transform.rotation = Quaternion.LookRotation(targetPos);
            _rigidBody.MovePosition(transform.position + targetPos);
        }
        else
            _animator.SetBool("isMoving", false);
    }
}
