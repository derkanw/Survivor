using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
<<<<<<< Updated upstream
    [Range(0f, 100f)] public float MaxHP;
    [HideInInspector] public Vector3 direction;
    [SerializeField] [Range(0f, 10f)] private float Speed;
    [SerializeField] private Transform bar;
=======
    public event Action<float> OnChangedHP;
    public event Action<Vector3> OnMoved;
    public event Action OnDied;

    [SerializeField] private Stat Health;
    [SerializeField] private Stat Mana;
    [SerializeField] private Stat Rapidity;
    [SerializeField] private Stat Agility;
    [SerializeField] private Stat Power;
>>>>>>> Stashed changes

    private Rigidbody _rigidBody;
    private Animator _animator;
    private Vector3 _position;
<<<<<<< Updated upstream
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
=======
    private BaseGun _gun;
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream
        if (collider.gameObject.tag == "Enemy")
        {
            _hp -= collider.gameObject.GetComponent<BaseEnemy>().Power;
            _healthBar.fillAmount = _hp / MaxHP;
        }
=======
>>>>>>> Stashed changes
        if (_hp <= 0)
        {
            Destroy(gameObject);
            bar.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        Health.Init();
        Mana.Init();
        Rapidity.Init();
        Agility.Init();
        Power.Init();

        _gun = GameObject.FindWithTag("Gun").GetComponent<BaseGun>(); // how it will be changed if there is a list of weapons?
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _animator = gameObject.GetComponent<Animator>();
        _hp = Health.Value;
    }

    public void TakeDamage(float power)
    {
        _hp -= power;
        OnChangedHP?.Invoke(_hp / Health.Value);
    }

    public void OnLevelUp(Dictionary<StatsNames, int> stats)
    {
        foreach (StatsNames name in stats.Keys)
            switch (name)
            {
                case StatsNames.Health:
                    Health.Modify(stats[name]);
                    break;
                case StatsNames.Mana:
                    Mana.Modify(stats[name]);
                    break;
                case StatsNames.Rapidity:
                    Rapidity.Modify(stats[name]);
                    break;
                case StatsNames.Agility:
                    Agility.Modify(stats[name]);
                    break;
                case StatsNames.Power:
                    Power.Modify(stats[name]);
                    break;
            }
        _gun.SetParams(Agility.Value, Power.Value);
    }

    public void ToMove(Vector3 position) => _position = position * Rapidity.Value;

    public void ToLook(Vector3 direction) => transform.LookAt(direction);
}
