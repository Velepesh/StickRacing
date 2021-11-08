using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Stick : MonoBehaviour
{
    [SerializeField] private StickConnectionCollider _stickCollider;
    [SerializeField] private GameObject _stick;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Transform _connectToStickPoint;

    private bool _isThrowing = false;
    private Vector3 _direction;
    private Rigidbody _rigidbody;

    public event UnityAction<Stick> Connected;
    public Transform ConnectToStickPoint => _connectToStickPoint;

    private void OnEnable()
    {
        _stickCollider.Connected += OnConnected;
    }

    private void OnDisable()
    {
        _stickCollider.Connected -= OnConnected;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = false;
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
        transform.SetParent(null);

        _isThrowing = true;
        _direction = (targetPoint - transform.position).normalized;
        transform.LookAt(targetPoint);
    }
    private void OnConnected()
    {
        _isThrowing = false;

        _rigidbody.isKinematic = true;
        Connected?.Invoke(this);
    }
}