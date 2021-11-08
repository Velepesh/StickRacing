using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform[] _hands;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _smoothTime = 0.5f;

    private Camera _camera;
    private Vector3 _velocity;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (_hands.Length == 0)
            return;

        Move();
    }

    private void Move() 
    { 
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + _offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref _velocity, _smoothTime);
    }

    private Vector3 GetCenterPoint()
    {
        if(_hands.Length == 1)
        {
            return _hands[0].transform.position;
        }

        var bounds = new Bounds(_hands[0].position, Vector3.zero);

        for (int i = 0; i < _hands.Length; i++)
        {
            bounds.Encapsulate(_hands[i].position);
        }

        return bounds.center;
    }
}
