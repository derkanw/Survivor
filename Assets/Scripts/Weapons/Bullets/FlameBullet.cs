using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBullet : BaseBullet
{
    private Vector3 _startPosition;
    private int _mask;
    private float _damageTime;
    private float _attackTime;
    private float _maxDistance;

    protected override void Move() => StartCoroutine(Damage());

    private void Start()
    {
        _startPosition = transform.position;
        _mask = 1 << 3;
        _damageTime = 1f;
        _attackTime = 1f;
        _maxDistance = 30f;
        Move();
    }

    private IEnumerator Damage()
    {
        float time = 0f;
        while (time <= _attackTime)
        {
            RaycastHit[] hits = Physics.RaycastAll(_startPosition, transform.forward, _maxDistance, _mask);
            foreach (var hit in hits)
                hit.collider.gameObject.GetComponent<BaseEnemy>().TakeDamage(Power);
            time += _damageTime;
            yield return new WaitForSeconds(_damageTime);
        }
    }
}
