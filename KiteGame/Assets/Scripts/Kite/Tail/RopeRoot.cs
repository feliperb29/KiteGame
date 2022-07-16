using System.Collections.Generic;
using UnityEngine;

public class RopeRoot : MonoBehaviour
{
    [SerializeField] private float rigidbodyMass = 1f;
    [SerializeField] private float colliderRadius = 0.1f;
    [SerializeField] private float jointSpring = 0.1f;
    [SerializeField] private float jointDamper = 5f;
    [SerializeField] private Vector3 rotationOffset;
    [SerializeField] private Vector3 positionOffset;

    private List<Transform> CopySource;
    private List<Transform> CopyDestination;
    private static GameObject rigidBodyContainer;

    private void Awake()
    {
        if (!rigidBodyContainer)
        {
            rigidBodyContainer = new GameObject("RopeRigidbodyContainer");
            rigidBodyContainer.transform.parent = transform.root;
        }

        CopySource = new List<Transform>();
        CopyDestination = new List<Transform>();

        AddChildren(transform);
    }

    private void AddChildren(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);
            var representative = new GameObject(child.gameObject.name);
            representative.transform.parent = rigidBodyContainer.transform;

            //rigidbody
            var childRigidbody = representative.gameObject.AddComponent<Rigidbody>();
            childRigidbody.useGravity = true;
            childRigidbody.isKinematic = false;
            childRigidbody.freezeRotation = true;
            childRigidbody.mass = rigidbodyMass;

            //collider
            var collider = representative.gameObject.AddComponent<SphereCollider>();
            collider.center = Vector3.zero;
            collider.radius = colliderRadius;

            //DistanceJoint
            var joint = representative.gameObject.AddComponent<DistanceJoint3D>();
            joint.connectedRigidbody = parent;
            joint.determineDistanceOnStart = true;
            joint.spring = jointSpring;
            joint.damper = jointDamper;
            joint.determineDistanceOnStart = false;
            joint.distance = Vector3.Distance(parent.position, child.position);

            //add copy source
            CopySource.Add(representative.transform);
            CopyDestination.Add(child);
            AddChildren(child);
        }
    }

    private void Update()
    {
        for (var i = 0; i < CopySource.Count; i++)
        {
            CopyDestination[i].position = CopySource[i].position + positionOffset;
            CopyDestination[i].rotation = CopySource[i].rotation * Quaternion.Euler(rotationOffset);
        }
    }
}