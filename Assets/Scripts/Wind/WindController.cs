using UnityEngine;

public class WindController : MonoBehaviour
{
    //TODO Maybe is better to use the value from the windSpeed in more than one axis only, this way will make more sense of how air physics works

    [SerializeField] private float windSpeedStartValue;
    [SerializeField] private Transform windDirectionStartValue;

    public float GlobalWindSpeed { get; private set; }
    public Transform GlobalWindDirection { get; private set; }
    
    private void Awake()
    {
        GlobalWindSpeed = windSpeedStartValue;
        GlobalWindDirection = windDirectionStartValue;
    }

    private void UpdateWindValues()
    {
        //Update the actual windSpeed and windDirection during the gameplay here
    }
}