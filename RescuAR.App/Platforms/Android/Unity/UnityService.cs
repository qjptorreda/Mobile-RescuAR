using System.Text.Json;

using Android.Content;
using Android.Util;

using RescuAR.App.Models;
using RescuAR.App.Services.Unity;

namespace RescuAR.App.Platforms.Android.Unity;

public sealed class UnityService : IUnityService
{
    private const string UnityPackageName =
        "com.rescuar.augmentedreality";

    public void LaunchUnity(EvacuationCenter center)
    {
        try
        {
            var activity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
            

            var packageManager =
                activity?.PackageManager;

            if (packageManager == null)
            {
                Log.Error(
                    "RESCUAR_UNITY",
                    "PackageManager is unavailable.");

                return;
            }

            var intent =
                packageManager.GetLaunchIntentForPackage(
                    UnityPackageName);

            if (intent == null)
            {
                Log.Error(
                    "RESCUAR_UNITY",
                    $"Unity application '{UnityPackageName}' " +
                    "is not installed.");

                return;
            }

            string json =
                JsonSerializer.Serialize(center);

            intent.PutExtra(
                "evacuation_center",
                json);

            intent.AddFlags(ActivityFlags.SingleTop);

            activity?.StartActivity(intent);

            Log.Debug(
                "RESCUAR_UNITY",
                "Unity application launched successfully.");
        }
        catch (Exception ex)
        {
            Log.Error(
                "RESCUAR_UNITY",
                $"Failed to launch Unity application: {ex}");
        }
    }
}
