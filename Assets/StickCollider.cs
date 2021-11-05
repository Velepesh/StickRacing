using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StickCollider : MonoBehaviour
{
    [SerializeField] private Stick _stick;
    [SerializeField] private Transform _connectionPoint;

    public event UnityAction Connected;

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 0.01f, Color.red);
        Debug.DrawRay(transform.position, transform.right * 0.01f, Color.blue);
        Debug.DrawRay(transform.position, transform.up * 0.01f, Color.green);
    }
    private void OnCollisionEnter(Collision collision)
    {
        //// creates joint
        //FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        //// sets joint position to point of contact
        //joint.anchor = collision.contacts[0].point;

        //Debug.Log(joint.anchor);
        //// conects the joint to the other object
        //joint.connectedBody = collision.contacts[0].otherCollider.transform.GetComponentInParent<Rigidbody>();
        //// Stops objects from continuing to collide and creating more joints
        //joint.enableCollision = false;

       
       // GetComponent<Rigidbody>().isKinematic = true;
        //Debug.Log(collision.gameObject.transform + " collision.gameObject.transform");
        //Debug.Log(gameObject.transform.position + " gameObject.transform");
        var normal = collision.contacts[0].normal;
        Debug.Log(collision.contacts[0].point + "collision.contacts[0].point");
        Debug.Log(normal + " normal");


        //var t = _stick.gameObject.transform.position - Vector3.Dot(forward, normal) * normal;

        //_stick.gameObject.transform.SetParent(collision.transform);

        //  _stick.gameObject.transform.position = t;

        //transform.rotation = Quaternion.FromToRotation(-transform.up, normal);
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.01f))
        //{
        //    Debug.Log(hit.collider.gameObject.name + "   name");
        //    transform.rotation = Quaternion.FromToRotation(-transform.up, normal);
        //    Debug.Log("RAY0");

        //}
        transform.rotation = Quaternion.FromToRotation(-transform.up, normal);
        transform.position = collision.contacts[0].point;
        _stick.gameObject.transform.SetParent(collision.gameObject.transform);
        GetComponent<Rigidbody>().isKinematic = true;
        //https://forum.unity.com/threads/projection-of-point-on-plane.855958/
        //https://stackoverflow.com/questions/69109173/force-an-ar-object-to-always-stand-upright-in-unity-vuforia
        //https://www.youtube.com/watch?v=KFUygjZKD8E&t=1448s
        //https://forum.unity.com/threads/align-object-to-walls.1190116/
        //https://answers.unity.com/questions/1181641/aligning-object-to-wall.html

        // if (normal != Vector3.zero)
        // {
        //     if ((int)normal.x != 0)
        //     {
        //        _stick.transform.rotation = Quaternion.LookRotation(Vector3.up, normal);
        //     }
        //     else
        //     {
        //        _stick.transform.rotation = Quaternion.LookRotation(Vector3.left, normal);
        //     }

        // }
        //_stick.transform.position += (normal * 0.05f); // This stops it from z-fighting with the object surface it lands on


        Connected?.Invoke();
    }

    

    //private void OnCollisionEnter(Collision col)
    //{
    //    Debug.Log("Collision");
    //    // creates joint
    //    FixedJoint joint = gameObject.AddComponent<FixedJoint>();
    //    // sets joint position to point of contact
    //    joint.anchor = col.contacts[0].point;
    //    Debug.Log(joint.anchor);
    //    // conects the joint to the other object
    //    joint.connectedBody = col.contacts[0].otherCollider.transform.GetComponentInParent<Rigidbody>();
    //    // Stops objects from continuing to collide and creating more joints
    //    joint.enableCollision = false;

    //    Connected?.Invoke();
    //}
}
