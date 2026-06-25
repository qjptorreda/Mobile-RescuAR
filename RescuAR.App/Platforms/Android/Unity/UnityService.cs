using Android.Content;
using RescuAR.App.Services.Unity;

namespace RescuAR.App.Platforms.Android.Unity
{
    public sealed class UnityService : IUnityService
    {
        public void LaunchUnity()
        {
            var activity = Platform.CurrentActivity;

            if (activity == null)
                return;

            var intent = new Intent();

            intent.SetClassName(
                "com.rescuar.ar",
                "com.unity3d.player.UnityPlayerGameActivity");

            activity.StartActivity(intent);
        }
    }
}
