using System.Collections;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Rigidbody))]
public class KiteController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private Transform player;

    private WindController _windController;
    private Vector3 _windDirectionV3;
    private float _tapCooldownTime = 0.5f;
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
        _canTap = true;
        _windController = FindObjectOfType<WindController>();
        
        if (!_windController)
        {
            Debug.LogError("Wind Controller GameObject not found.");
            EditorApplication.isPlaying = false;
        }
    }

    private void Update()
    {
        SetKiteDirection();

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
        //TODO fix the addforce mechanic and direction of the kite, by the reason right now for testing purposes the values are hardcoded to 4 and -4
        //and also the way its working at the same time with MoveKite call in Update() and FixedUpdate() is very bad 
    }

    private void FixedUpdate()
    {
        _rb.AddForce(_windDirectionV3 * _windController.GlobalWindSpeed, ForceMode.Impulse);
        //_rb.AddForce(new Vector2(_windDirectionV3.x * _globalWindSpeed, 0), ForceMode.Impulse);

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
        
        while (currentTime < _tapCooldownTime)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }

        _canTap = true;
    }

    private void TapSystem()
    {
        if (!_canTap)
        {
            return;
        }

        _canTap = false;
        Debug.Log("TAP");
        _movement = new Vector2(-Input.GetAxisRaw("Horizontal"), Input.GetAxis("Vertical"));
        StartCoroutine(LerpCooldownValue());
    }

    private void SetKiteDirection()
    {
        _windDirectionV3 = new Vector3(_windController.GlobalWindDirection.transform.position.x - transform.position.x, _windController.GlobalWindSpeed,
            _windController.GlobalWindDirection.transform.position.z - transform.position.z);
       
        _windDirectionV3 = _windDirectionV3.normalized;
    }
}