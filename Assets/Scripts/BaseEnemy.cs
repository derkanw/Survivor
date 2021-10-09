using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseEnemy : MonoBehaviour
{
    [Range(0f, 100f)] public float MaxHP;
    [Range(0f, 100f)] public float Power;
    [SerializeField] [Range(0f, 10f)] private float Speed;

    private GameObject _player;
    private Rigidbody _rigidBody;
    private Vector3 _position;
    private Vector3 _offset;
    private Animator _animator;
    private Image _healthBar;
    private float _hp;

    private void Start()
    {
        _hp = MaxHP;
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _player = GameObject.FindWithTag("Player");
        _animator = gameObject.GetComponent<Animator>();
        _healthBar = transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Bullet")
        {
            _hp -= collider.gameObject.GetComponent<BaseBullet>().Power;
            _healthBar.fillAmount = _hp / MaxHP;
        }
        if (_hp <= 0)
        {
            GameObject.Find("manager").GetComponent<EnemiesManager>().KilledCount++;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*if (collision.gameObject.tag == "Enemy")
        //need to fix collision between enemies
        */
    }

    private void FixedUpdate()
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
