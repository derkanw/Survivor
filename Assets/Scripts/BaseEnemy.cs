using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseEnemy : MonoBehaviour
{
<<<<<<< Updated upstream
    [Range(0f, 100f)] public float MaxHP;
    [Range(0f, 100f)] public float Power;
    [SerializeField] [Range(0f, 10f)] private float Speed;
=======
    public event Action<BaseEnemy, float> OnEnemyDied;

    [SerializeField] private Stat Health;
    [SerializeField] private Stat Rapidity;
    [SerializeField] private Stat Power;
    [SerializeField] private Stat DeathPoints;
>>>>>>> Stashed changes

    private GameObject _player;
    private Rigidbody _rigidBody;
    private Vector3 _position;
    private Vector3 _offset;
    private Animator _animator;
    private Image _healthBar;
    private float _hp;

    private void Start()
    {
        Health.Init();
        Rapidity.Init();
        Power.Init();
        DeathPoints.Init();

        _hp = Health.Value;
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _player = GameObject.FindWithTag("Player");
        _animator = gameObject.GetComponent<Animator>();
        _healthBar = transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
    }

    private void OnTriggerEnter(Collider collider)
    {
<<<<<<< Updated upstream
        if (collider.gameObject.tag == "Bullet")
        {
            _hp -= collider.gameObject.GetComponent<BaseBullet>().Power;
            _healthBar.fillAmount = _hp / MaxHP;
        }
        if (_hp <= 0)
        {
            GameObject.Find("manager").GetComponent<EnemiesManager>().KilledCount++;
=======
        var target = collider.gameObject;
        if (target.tag == "Player")
            target.GetComponent<Player>().TakeDamage(Power.Value);
        if (_hp <= 0)
        {
            OnEnemyDied?.Invoke(this, DeathPoints.Value);
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        else
            _animator.SetBool("isMoving", false);
=======
    }

    public void TakeDamage(float power)
    {
        _hp -= power;
        _healthBar.fillAmount = _hp / Health.Value;
    }

    public void OnLevelUp(int level)
    {
        Health.Modify(level);
        Rapidity.Modify(level);
        Power.Modify(level);
        DeathPoints.Modify(level);
    }

    public void ToMove(Vector3 position) => _targetPosition = (position - transform.position) * Rapidity.Value;

    public void ToStay()
    {
        _isPlayerExist = false;
        _animator.SetBool("isMoving", false);
>>>>>>> Stashed changes
    }
}
