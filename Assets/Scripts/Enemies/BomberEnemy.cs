using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberEnemy : BaseEnemy
{
    [SerializeField] private GameObject Effect;

    private IEnumerator Attack(Collider target)
    {
        _animator.SetTrigger("Attack");
        Instantiate(Effect, transform.position, Quaternion.identity);
        if (target != null)
            target.GetComponent<Player>().TakeDamage(Power.Value);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Died());
    }

    private void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<Collider>();
        if (target.CompareTag("Player"))
            StartCoroutine(Attack(target));
    }

    private void FixedUpdate()
    {
        if (_isPlayerExists)
        {
            Vector3 targetPos = _targetPosition * Rapidity.Value * Time.fixedDeltaTime;
            transform.rotation = Quaternion.LookRotation(targetPos);
            _rigidBody.MovePosition(transform.position + targetPos);
        }
    }
}
