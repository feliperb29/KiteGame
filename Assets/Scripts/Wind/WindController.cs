using UnityEngine;

public class WindController : MonoBehaviour
{
    //Maybe is better to specify the windSpeed to different axis, to make more sense of how air physics works

    [SerializeField] private float windSpeedStartValue;
    [SerializeField] private Transform windDirectionStartValue;

    public float GlobalWindSpeed { get; private set; }
    public Transform GlobalWindDirection { get; private set; }
    
    private void Awake()
    {
        GlobalWindSpeed = windSpeedStartValue;
        GlobalWindDirection = windDirectionStartValue;
    }

    private void UpdateWind()
    {
        //Here update the actual windSpeed and windDirection during the gameplay
    }
}