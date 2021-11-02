using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBullet : BaseBullet
{
    private Vector3 _startPosition;
    private int _mask;
    private float _damageTime;
    private float _attackTime;

    protected override void Move() => StartCoroutine(Damage());

    private void Start()
    {
        _startPosition = transform.position;
        _mask = 1 << 3;
        _damageTime = 1f;
        _attackTime = 1f;
        Move();
    }

    private IEnumerator Damage()
    {
        float time = 0f;
        while (time <= _attackTime)
        {
            if (Physics.Raycast(_startPosition, transform.forward, out RaycastHit hit, 30f, _mask))
                hit.collider.gameObject.GetComponent<BaseEnemy>().TakeDamage(Power);
            time += _damageTime;
            yield return new WaitForSeconds(_damageTime);
        }
    }
}
