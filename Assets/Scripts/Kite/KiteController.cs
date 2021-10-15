using System.Collections;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Rigidbody))]
public class KiteController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float tapCooldownTime = 0.5f;
    [SerializeField] private Transform player;

    private WindController _windController;
    private Vector3 _windDirectionV3;
    private Transform _kiteTransform;
    private Rigidbody _rigidBody;
    private Vector2 _movement;
    private bool _canTap;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _kiteTransform = GetComponent<Transform>();
    }

    private void Start()
    {
        _canTap = true;
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
        _movement = new Vector2(0, Input.GetAxis("Vertical"));

        //TODO fix the addforce mechanic and direction of the kite, by the reason right now for testing purposes the values are hardcoded to 4 and -4
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveKite(new Vector2(4, 4));
        }
        
        else if(Input.GetKeyDown(KeyCode.D))
        {
            MoveKite(new Vector2(-4, 4));
        }
    }

    private void FixedUpdate()
    {
        _rigidBody.AddForce(_windDirectionV3 * _windController.GlobalWindSpeed, ForceMode.Impulse);
        MoveKite(_movement);
    }

    private void MoveKite(Vector2 direction)
    {
        //Maybe the hardcoded values here could be serialized to be easier to update it in the inspector.
        _rigidBody.AddForce(new Vector2(direction.x * 4, direction.y), ForceMode.Impulse);
        _rigidBody.velocity *= 0.9f;
        _rigidBody.angularVelocity *= 0.9f;
    }

    private IEnumerator LerpCooldownValue()
    {
        float currentTime = 0;
        
        while(currentTime < tapCooldownTime)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }

        _canTap = true;
    }

    private void SetKiteDirection()
    {
        _windDirectionV3 = new Vector3(_windController.GlobalWindDirection.transform.position.x - transform.position.x, _windController.GlobalWindSpeed,
            _windController.GlobalWindDirection.transform.position.z - transform.position.z);
       
        _windDirectionV3 = _windDirectionV3.normalized;
    }
}