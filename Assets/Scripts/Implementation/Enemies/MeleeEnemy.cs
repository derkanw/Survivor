using System.Collections;
using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    private IEnumerator Damage(GameObject target)
    {
        if (_isPlayerExists && _isAttacking)
        {
            _animator.SetTrigger("Attack");
            yield return new WaitForSeconds(TimeOffset);
            AudioManager.PlaySound(SoundNames.EnemyAttack);
            if (target != null)
                target.GetComponent<IPlayer>().TakeDamage(Power.Value);
            yield return new WaitForSeconds(AttackSpeed);
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
