using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Range(0f, 100f)] public float MaxHP;
    [HideInInspector] public Vector3 direction;
    [SerializeField] [Range(0f, 10f)] private float Speed;
    [SerializeField] private Transform bar;

    private Rigidbody _rigidBody;
    private Animator _animator;
    private Vector3 _position;
    private float _hp;
    private Image _healthBar;

    private void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _animator = gameObject.GetComponent<Animator>();
        _hp = MaxHP;
        _healthBar = bar.GetChild(2).transform.GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        _position.x = Input.GetAxisRaw("Horizontal");
        _position.z = Input.GetAxisRaw("Vertical");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit rh))
            direction = rh.point;
        direction.y = 0;
        transform.LookAt(direction);
    }

    private void FixedUpdate()
    {
        if (_position == Vector3.zero)
            _animator.SetBool("isMoving", false);
        else
        {
            _animator.SetBool("isMoving", true);
            _rigidBody.MovePosition(transform.position + _position * Speed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            _hp -= collider.gameObject.GetComponent<BaseEnemy>().Power;
            _healthBar.fillAmount = _hp / MaxHP;
        }
        if (_hp <= 0)
        {
            Destroy(gameObject);
            bar.gameObject.SetActive(false);
        }
    }
}
