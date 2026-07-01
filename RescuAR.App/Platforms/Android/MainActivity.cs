using Android.App;
using Android.Content.PM;
using Android.OS;
using RescuAR.App.Platforms.Android.Unity;

namespace RescuAR.App.Platforms.Android;

[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges =
        ConfigChanges.ScreenSize |
        ConfigChanges.Orientation |
        ConfigChanges.UiMode |
        ConfigChanges.ScreenLayout |
        ConfigChanges.SmallestScreenSize |
        ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(
        Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        UnityVerifier.Verify();

        NativeLibraryVerifier.Verify();
    }
}
