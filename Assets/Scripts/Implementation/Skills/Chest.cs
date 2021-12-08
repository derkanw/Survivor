using System.Collections;
using UnityEngine;
using System;

public class Chest : MonoBehaviour, IChest
{
    public event Action BoxDestroyed;
    [SerializeField] private GameObject Box;
    [SerializeField] private GameObject[] Things;
    [SerializeField] [Range(0f, 1f)] private float Chance;

    private Animator _animator;
    private Vector3 _offset;
    private bool _isOpen;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isOpen)
        {
            _isOpen = true;
            _animator.SetTrigger("Open");
            AudioManager.PlaySound(SoundNames.ChestOpen);
            if (UnityEngine.Random.Range(0f, 1f) >= (1 - Chance))
            {
                var index = UnityEngine.Random.Range(0, Things.Length);
                Instantiate(Things[index], transform.position + _offset, Quaternion.identity);
            }
            Box.SetActive(false);
            StartCoroutine(DestroyBox());
        }
    }

    private IEnumerator DestroyBox()
    {
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        BoxDestroyed?.Invoke();
        Destroy(gameObject);
    }
    private void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _offset = new Vector3(0f, 0.5f, 0f);
    }
}
