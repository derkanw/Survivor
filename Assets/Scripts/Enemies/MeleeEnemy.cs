using System.Collections;
using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    private float _attackSpeed = 5f;

    private IEnumerator Damage(GameObject target)
    {
        if (_isPlayerExists && _isAttacking)
        {
            _animator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.8f);
            AudioManager.PlaySound(SoundNames.EnemyAttack);
            if (target != null)
                target.GetComponent<Player>().TakeDamage(Power.Value);
            yield return new WaitForSeconds(_attackSpeed);
            _isAttacking = false;
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        var target = collider.gameObject;
        if (target.CompareTag("Player") && !_isAttacking)
        {
            _isAttacking = true;
            _animator.SetBool("isMoving", false);
            StartCoroutine(Damage(target));
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        var target = collider.gameObject;
        if (!target.CompareTag("Player")) return;
        _isAttacking = false;
        StopCoroutine(Damage(collider.gameObject));
        _animator.SetBool("isMoving", true);
    }

    private void FixedUpdate()
    {
        if (_isPlayerExists)
        {
            transform.rotation = Quaternion.LookRotation(_targetPosition);
                _navMesh.destination = transform.position + _targetPosition;
        }
    }
}
