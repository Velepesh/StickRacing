using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class StickCollider : MonoBehaviour
{
    [SerializeField] private Stick _stick;

    private Rigidbody _rigidbody;

    public event UnityAction Connected;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var contact = collision.contacts[0];
        var normal = contact.normal;

        transform.position = contact.point;
        transform.rotation = Quaternion.FromToRotation(-transform.right, normal) * transform.rotation;
       
        _stick.gameObject.transform.SetParent(collision.gameObject.transform);
        _rigidbody.isKinematic = true;

        Connected?.Invoke();
    }
}