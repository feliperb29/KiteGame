using System;
using UnityEngine;

public class KiteController : MonoBehaviour
{
    [SerializeField] private Transform kiteTransform;
    private Transform _playerTransform;
    private float _speed = 0.1f;

    private void Awake()
    {
        _playerTransform = GetComponent<Transform>();
    }
}
