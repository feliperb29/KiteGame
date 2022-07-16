using UnityEngine;

public class WindController : MonoBehaviour
{
    //TODO Maybe is better to use the value from the windSpeed in more than one axis only, this way will make more sense of how air physics works

    [SerializeField] private float windSpeedStartValue;
    [SerializeField] private Transform windDirectionStartValue;

    public float GlobalWindSpeed { get; private set; }
    public Transform GlobalWindDirection { get; private set; }
    
    private void Awake() => UpdateWindSpecs(windSpeedStartValue, windDirectionStartValue);

    //TODO A time counter to update the wind specs by the time passed ingame
    private void UpdateWindSpecs(float windSpeedReceived, Transform windDirectionReceived)
    {
        GlobalWindSpeed = windSpeedReceived;
        GlobalWindDirection = windDirectionReceived;
    }
}