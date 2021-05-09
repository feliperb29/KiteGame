using UnityEngine;

public class WindController : MonoBehaviour
{
    //maybe is better to specify the windSpeed to different axis, to make more sense of how air physics works

    [SerializeField] private float windSpeed;
    [SerializeField] private Transform windDirection;

    public float GetGlobalWindSpeed() => windSpeed;
    
    public Transform GetGlobalWindDirection() => windDirection;
}