using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KiteController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;

    private Rigidbody _rb;
    private Vector2 _movement;
    
    private void Awake() => _rb = GetComponent<Rigidbody>();

    private void Update()
    {
        Debug.Log(Input.GetAxis("Horizontal"));
        _movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    } 

    private void FixedUpdate()
    {
        
        MoveKite(_movement);
    }

    private void MoveKite(Vector2 direction)
    {
        _rb.AddForce(direction * speed);
        
        if (_rb.velocity.magnitude > maxSpeed)
            _rb.velocity = _rb.velocity.normalized * maxSpeed;
    }
}
