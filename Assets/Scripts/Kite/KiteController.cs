using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KiteController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private Transform player;

    private WindController _windController;
    private Vector3 _windDirectionV3;
    private float _globalWindSpeed;
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

    private void Start()
    {
        _windController = FindObjectOfType<WindController>();
        _canTap = true;
    }

    private void Update()
    {
        SetKiteWindDirection();

        _kiteTransform.LookAt(player);
        _movement = new Vector2(0, Input.GetAxis("Vertical"));

        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveKite(new Vector2(4, 4));
        }
        
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveKite(new Vector2(-4, 4));
        }
        //TODO fix the addforce direction of the kite, by the reason right now for testing purposes the values are hardcoded to 4 and -4
    }

    private void FixedUpdate()
    {
        _rb.AddForce(_windDirectionV3 * _globalWindSpeed, ForceMode.Impulse);

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

    private void SetKiteWindDirection()
    {
        _globalWindSpeed = _windController.GetGlobalWindSpeed();
        var globalWindDirection = _windController.GetGlobalWindDirection();

        _windDirectionV3 = new Vector3(globalWindDirection.transform.position.x - transform.position.x, _globalWindSpeed,
            globalWindDirection.transform.position.z - transform.position.z);
        _windDirectionV3 = _windDirectionV3.normalized;
    }
}