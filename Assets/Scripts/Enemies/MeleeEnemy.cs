using System.Collections;
using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    private float _attackSpeed = 5f;
    private IEnumerator Damage(GameObject target)
    {
        while (_isPlayerExists && _isAttacking)
        {
            _animator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.8f);
            AudioManager.PlaySound(SoundNames.EnemyAttack);
            if (target != null)
                target.GetComponent<Player>().TakeDamage(Power.Value);
            yield return new WaitForSeconds(_attackSpeed);
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
        _isAttacking = false;
        _animator.SetBool("isMoving", true);
    }

    private void FixedUpdate()
    {
        if (_isPlayerExists)
        {
            Vector3 targetPos = _targetPosition * Rapidity.Value * Time.fixedDeltaTime;
            transform.rotation = Quaternion.LookRotation(targetPos);
            if (!_isAttacking)
                _rigidBody.MovePosition(transform.position + targetPos);
        }
    }
}
