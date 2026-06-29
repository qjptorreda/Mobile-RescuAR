using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public class LocationServiceManager : MonoBehaviour
{
    private IEnumerator Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR

        if (!Permission.HasUserAuthorizedPermission(
                Permission.FineLocation))
        {
            Permission.RequestUserPermission(
                Permission.FineLocation);

            while (!Permission.HasUserAuthorizedPermission(
                       Permission.FineLocation))
            {
                yield return null;
            }
        }

#endif

        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError(
                "GPS DISABLED");

            yield break;
        }

        Debug.Log(
            "STARTING GPS...");

        Input.location.Start(
            1f, // desiredAccuracyInMeters
            1f  // updateDistanceInMeters
        );

        int waitTime = 20;

        while (
            Input.location.status ==
            LocationServiceStatus.Initializing
            &&
            waitTime > 0)
        {
            Debug.Log(
                $"GPS INITIALIZING... {waitTime}");

            yield return new WaitForSeconds(1);

            waitTime--;
        }

        if (
            Input.location.status ==
            LocationServiceStatus.Failed)
        {
            Debug.LogError(
                "GPS FAILED");

            yield break;
        }

        Debug.Log(
            $"GPS STARTED. STATUS={Input.location.status}");

        InvokeRepeating(
            nameof(UpdateLocation),
            1f,
            1f);
    }

    private void UpdateLocation()
    {
        if (Input.location.status !=
            LocationServiceStatus.Running)
        {
            Debug.LogWarning(
                $"GPS NOT RUNNING: {Input.location.status}");

            return;
        }

        LocationInfo info =
            Input.location.lastData;

        NavigationManager.Instance.CurrentLocation =
            new UserLocationData
            {
                Latitude = info.latitude,
                Longitude = info.longitude
            };

        Debug.Log(
            $"GPS UPDATE | " +
            $"LAT={info.latitude} | " +
            $"LON={info.longitude} | " +
            $"ACC={info.horizontalAccuracy}m | " +
            $"TIME={info.timestamp}");
    }

    private void OnDestroy()
    {
        Input.location.Stop();
    }
}
