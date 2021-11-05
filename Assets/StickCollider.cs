using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class StickCollider : MonoBehaviour
{
    [SerializeField] private Stick _stick;
    [SerializeField] private Transform _connectionPoint;

    private Rigidbody _rigidbody;

    public event UnityAction Connected;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var contactPoint = collision.contacts[0];
        var normal = contactPoint.normal;

        transform.rotation = Quaternion.FromToRotation(-transform.up, normal);
        transform.position = contactPoint.point;
       
        _stick.gameObject.transform.SetParent(collision.gameObject.transform);
        _rigidbody.isKinematic = true;

        Connected?.Invoke();
    }
}