using System.Collections;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Rigidbody))]
public class KiteController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float tapCooldownTime;
    [SerializeField] private Transform player;

    private WindController _windController;
    private Vector3 _windDirection;
    private Vector2 _verticalkiteMovement;
    private Transform _kiteTransform;
    private Rigidbody _rigidBody;
    private WaitForSeconds _waitForTap;
    private bool _canTap;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _kiteTransform = GetComponent<Transform>();
        _waitForTap = new WaitForSeconds(tapCooldownTime);
        _canTap = true;
    }

    private void Start()
    {
        _windController = FindObjectOfType<WindController>();
        
        if(!_windController)
        {
            Debug.LogError("Wind Controller GameObject not found.");
            EditorApplication.isPlaying = false;
        }
    }

    private void Update()
    {
        SetKiteDirection();
        _kiteTransform.LookAt(player);
        _verticalkiteMovement = new Vector2(0, Input.GetAxis("Vertical"));

        //TODO fix the addforce mechanic and direction of the kite, by the reason right now for testing purposes the values are hardcoded to 4 and -4
        if (_canTap && Input.GetKeyDown(KeyCode.A))
        {
            MoveKite(new Vector2(4, 4));
            StartCoroutine(WaitForTapCooldown());
        }
        
        else if(_canTap && Input.GetKeyDown(KeyCode.D))
        {
            MoveKite(new Vector2(-4, 4));
            StartCoroutine(WaitForTapCooldown());
        }
    }

    private void FixedUpdate()
    {
        _rigidBody.AddForce(_windDirection * _windController.GlobalWindSpeed, ForceMode.Impulse);
        MoveKite(_verticalkiteMovement);
    }

    private void MoveKite(Vector2 direction)
    {
        //Maybe the hardcoded numbers here could be serialized to be easier to update it in the inspector.
        _rigidBody.AddForce(new Vector2(direction.x * 4, direction.y), ForceMode.Impulse);
        _rigidBody.velocity *= 0.9f;
        _rigidBody.angularVelocity *= 0.9f;
    }

    private IEnumerator WaitForTapCooldown()
    {
        _canTap = false;
        yield return _waitForTap;
        _canTap = true;
    }

    private void SetKiteDirection()
    {
        _windDirection = new Vector3(_windController.GlobalWindDirection.transform.position.x - transform.position.x, _windController.GlobalWindSpeed,
            _windController.GlobalWindDirection.transform.position.z - transform.position.z);
       
        _windDirection = _windDirection.normalized;
    }
}