using System.Collections;
using UnityEngine;

public class BomberEnemy : BaseEnemy
{
    [SerializeField] private GameObject Effect;

    private IEnumerator Attack(Collider target)
    {
        _animator.SetTrigger("Attack");
        Instantiate(Effect, transform.position, Quaternion.identity);
        AudioManager.PlaySound(SoundNames.GrenadeExplosion);
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
            transform.rotation = Quaternion.LookRotation(_targetPosition);
            _navMesh.destination = transform.position + _targetPosition;
        }
    }
}
