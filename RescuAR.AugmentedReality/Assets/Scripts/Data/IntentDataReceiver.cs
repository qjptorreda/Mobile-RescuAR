using TMPro;
using UnityEngine;

public class IntentDataReceiver : MonoBehaviour
{
    [SerializeField]
    private TMP_Text outputText;

    private void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR

using var unityPlayer =
    new AndroidJavaClass(
        "com.unity3d.player.UnityPlayer");

using var activity =
    unityPlayer.GetStatic<AndroidJavaObject>(
        "currentActivity");

using var intent =
    activity.Call<AndroidJavaObject>(
        "getIntent");

string json =
    intent.Call<string>(
        "getStringExtra",
        "evacuation_center");

Debug.Log(json);

if (!string.IsNullOrEmpty(json))
{
    EvacuationCenterData center =
        JsonUtility.FromJson
        <
            EvacuationCenterData
        >(json);

    if (NavigationManager.Instance != null)
    {
        NavigationManager.Instance.Destination =
            new DestinationData
            {
                Name = center.Name,
                Latitude = center.Latitude,
                Longitude = center.Longitude
            };
    }

    outputText.text =
        $"{center.Name}\n" +
        $"Status: {center.Status}\n" +
        $"Occupancy: {center.Occupancy}/{center.Capacity}";
}

#endif
    }
}
