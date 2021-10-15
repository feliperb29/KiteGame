using UnityEngine;

public class DistanceJoint3D : MonoBehaviour {

    public Transform connectedRigidbody;
    public bool determineDistanceOnStart = true;
    public float distance;
    public float spring = 0.1f;
    public float damper = 5f;

    private Rigidbody _rigidbody;

    private void Awake() => _rigidbody = GetComponent<Rigidbody>();

    private void Start()
    {
        if (determineDistanceOnStart && connectedRigidbody)
        {
            distance = Vector3.Distance(_rigidbody.position, connectedRigidbody.position);
        }
    }

    private void FixedUpdate()
    {
        var connection = _rigidbody.position - connectedRigidbody.position;
        var distanceDiscrepancy = distance - connection.magnitude;

        _rigidbody.position += distanceDiscrepancy * connection.normalized;

        var velocityTarget = connection + (_rigidbody.velocity + Physics.gravity * spring);
        var projectOnConnection = Vector3.Project(velocityTarget, connection);
        _rigidbody.velocity = (velocityTarget - projectOnConnection) / (1 + damper * Time.fixedDeltaTime);
    }
}
