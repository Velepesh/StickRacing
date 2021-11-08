using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class StickConnectionCollider : MonoBehaviour
{
    [SerializeField] private Stick _stick;

    private Rigidbody _rigidbody;
    private Collider _collider;

    public event UnityAction Connected;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var contact = collision.contacts[0];
        var normal = contact.normal;
      
        transform.position = new Vector3(Mathf.Round(contact.point.x * 100f) / 100f, Mathf.Round(contact.point.y * 100f) / 100f, Mathf.Round(contact.point.z * 100f) / 100f);
        transform.rotation = Quaternion.FromToRotation(-transform.right, normal) * transform.rotation;
       
        _stick.gameObject.transform.SetParent(collision.gameObject.transform);
        _rigidbody.isKinematic = true;

        Connected?.Invoke();

        _collider.enabled = false;
    }
}