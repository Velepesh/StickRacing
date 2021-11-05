using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    [SerializeField] private StickCollider _stickCollider;
    [SerializeField] private GameObject _stick;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private DynamicBone _dynamicBone;

    private bool _isThrowing = false;
    private Vector3 _direction;
    private float _stickRotationZ;
    public Vector3 Direction => _direction;

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
        _stickRotationZ = _stick.transform.position.z;
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
       // _dynamicBone.enabled = false;
        _direction = (targetPoint - transform.position).normalized;
        transform.LookAt(targetPoint);
    }
    private void OnConnected()
    {
        _dynamicBone.enabled = true;
        _isThrowing = false;
        //https://answers.unity.com/questions/1534732/how-to-make-two-colliders-or-rigidbodies-stick-tog.html
    }
}