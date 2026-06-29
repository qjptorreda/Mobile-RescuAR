using System.Text.Json;
using Android.Content;
using RescuAR.App.Models;
using RescuAR.App.Services.Unity;

namespace RescuAR.App.Platforms.Android.Unity;

public sealed class UnityService : IUnityService
{
    public void LaunchUnity(
        EvacuationCenter center)
    {
        var activity =
            Platform.CurrentActivity;

        if (activity == null)
            return;

        var intent = new Intent();

        intent.SetClassName(
            "com.rescuar.augmentedreality",
            "com.unity3d.player.UnityPlayerGameActivity");

        string json =
            JsonSerializer.Serialize(center);

        intent.PutExtra(
            "evacuation_center",
            json);

        activity.StartActivity(intent);
    }
}
