using Android.Content;
using RescuAR.App.Services.Unity;

namespace RescuAR.App.Platforms.Android.Unity;

public sealed class UnityService : IUnityService
{
    public void LaunchUnity(
        string destinationName,
        double latitude,
        double longitude)
    {
        var activity = Platform.CurrentActivity;

        if (activity == null)
            return;

        var intent = new Intent();

        intent.SetClassName(
            "com.rescuar.ar",
            "com.unity3d.player.UnityPlayerGameActivity");

        intent.PutExtra(
            "destination_name",
            destinationName);

        intent.PutExtra(
            "destination_latitude",
            latitude);

        intent.PutExtra(
            "destination_longitude",
            longitude);

        activity.StartActivity(intent);
    }
}
