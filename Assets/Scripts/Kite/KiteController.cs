using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KiteController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private Transform player;
    private float tapCooldownTime = 0.5f;

    private Transform _kiteTransform;
    private Rigidbody _rb;
    private Vector2 _movement;
    private bool _canTap;

    private void Awake()
    { 
        _rb = GetComponent<Rigidbody>();
        _kiteTransform = GetComponent<Transform>();
    } 

    private void Start() => _canTap = true;
    
    private void Update()
    {
        _kiteTransform.LookAt(player);
        _movement = new Vector2(0, Input.GetAxis("Vertical"));

        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveKite(new Vector2(4,4));
        }
        
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveKite(new Vector2(-4,4));
        }
    } 

    private void FixedUpdate()
    {
        MoveKite(_movement);
    }

    private void MoveKite(Vector2 direction)
    {
        _rb.AddForce(new Vector2(direction.x * 4, direction.y), ForceMode.Impulse);
        _rb.velocity *= 0.9f;
        _rb.angularVelocity *= 0.9f;
    }

    private IEnumerator LerpCooldownValue()
    {
        float currentTime = 0;
        while (currentTime < tapCooldownTime)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        _canTap = true;
    }

    private void TapSystem()
    {
        if (!_canTap)
            return;
        _canTap = false;
        Debug.Log("TAP");
        _movement = new Vector2(-Input.GetAxisRaw("Horizontal"), Input.GetAxis("Vertical"));
        StartCoroutine(LerpCooldownValue());
    }
}
