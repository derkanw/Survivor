using System.Collections;
using UnityEngine;

public class ShooterEnemy : BaseEnemy
{
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private GameObject Effect;
    [SerializeField] [Range(0f, 50f)] private float ReloadTime;
    [SerializeField] [Range(0f, 20f)] private float ShootingSpeed;
    [SerializeField] [Range(0f, 100f)] private int ClipSize;
    [SerializeField] [Range(0f, 10f)] private float ShootDistance;
    [SerializeField] [Range(0f, 10f)] private float StayDistance;
    [SerializeField] private Transform BulletSpawn;

    private int _bulletsCount;
    private bool _close;
    private Vector3 _offset;

    private void InitBullet()
    {
        Instantiate(Effect, BulletSpawn.position, transform.rotation * Quaternion.Euler(90f, 0f, 90f));
        GameObject bullet = Instantiate(BulletPrefab, BulletSpawn.position, Quaternion.identity);
        bullet.transform.LookAt(_playerPosition + _offset);
        bullet.GetComponent<BaseBullet>().SetPower(Power.Value);
    }

    private void Awake()
    {
        _bulletsCount = ClipSize;
        _offset = new Vector3(0f, 0.75f, 0f);
    }

    private IEnumerator Attack()
    {
        _isAttacking = true;
        _animator.SetBool("isMoving", false);
        while (_close)
        {
            while (_close && _isPlayerExists && _bulletsCount != 0)
            {
                _animator.SetTrigger("Attack");
                InitBullet();
                --_bulletsCount;
                yield return new WaitForSeconds(ShootingSpeed);
            }
            yield return new WaitForSeconds(ReloadTime);
            _bulletsCount = ClipSize;
        }
        _animator.SetBool("isMoving", true);
        _isAttacking = false;
    }

    private void FixedUpdate()
    {
        if (_isPlayerExists)
        {
            transform.rotation = Quaternion.LookRotation(_targetPosition);
            _close = (_playerPosition - transform.position).magnitude <= ShootDistance;
            bool mustStay = (_playerPosition - transform.position).magnitude <= StayDistance;
            _navMesh.destination = mustStay ? transform.position : transform.position + _targetPosition;
            if (_close && !_isAttacking)
                StartCoroutine(Attack());
        }
    }
}
