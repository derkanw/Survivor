using UnityEngine;

public class CameraMovement : MonoBehaviour, IMovable
{
    [SerializeField] [Range(0f, 1f)] private float SmoothSpeed;

    private Vector3 _offset;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _toPlayer;
    private bool _canUse;

    public void MoveTo(Vector3 position) => _toPlayer = position;

    public void Stay() => _canUse = false;

    private void Start()
    {
        _offset = transform.position;
        _canUse = true;
    }

    private void LateUpdate()
    {
        if (_canUse)
        {
            Vector3 targetPos = _toPlayer + _offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, SmoothSpeed * Time.deltaTime);
        }
    }
}
