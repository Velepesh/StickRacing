using UnityEngine;

public class Stick : MonoBehaviour
{
    [SerializeField] private StickCollider _stickCollider;
    [SerializeField] private GameObject _stick;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;

    private bool _isThrowing = false;
    private Vector3 _direction;
    public Vector3 Direction => _direction;

    private void OnEnable()
    {
        _stickCollider.Connected += OnConnected;
    }

    private void OnDisable()
    {
        _stickCollider.Connected -= OnConnected;
    }

    private void Update()
    {
        if (_isThrowing)
        {
            transform.Translate(_direction * _speed * Time.deltaTime, Space.World);

            _stick.transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0, Space.Self);
        }
    }

    public void Throwing(Vector3 targetPoint)
    {
        _isThrowing = true;
        _direction = (targetPoint - transform.position).normalized;
        transform.LookAt(targetPoint);
    }
    private void OnConnected()
    {
        _isThrowing = false;
    }
}