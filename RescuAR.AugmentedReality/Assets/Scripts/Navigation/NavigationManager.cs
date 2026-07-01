using TMPro;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    public static NavigationManager Instance;

    [Header("Debug UI")]
    [SerializeField]
    private TMP_Text navigationText;

    public DestinationData Destination;

    public UserLocationData CurrentLocation;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        UpdateNavigation();
    }

    private void UpdateNavigation()
    {
        if (Destination == null)
            return;

        if (CurrentLocation == null)
            return;

        double distance =
            NavigationUtility.CalculateDistanceMeters(
                CurrentLocation.Latitude,
                CurrentLocation.Longitude,
                Destination.Latitude,
                Destination.Longitude);

        double bearing =
            NavigationUtility.CalculateBearing(
                CurrentLocation.Latitude,
                CurrentLocation.Longitude,
                Destination.Latitude,
                Destination.Longitude);

        bool arrived =
            distance <= 25.0;

        navigationText.text =
            $"Destination: {Destination.Name}\n\n" +
            $"Distance: {distance:F2} m\n" +
            $"Bearing: {bearing:F2}°\n" +
            $"Arrived: {arrived}";
    }
}
