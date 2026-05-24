#if ANDROID
using Android.Content;
using Android.App;
#endif

namespace RescuAR.App.Views;

public partial class CameraPage : ContentPage
{
    public CameraPage()
    {
        InitializeComponent();
    }

    private async void OnLaunchARClicked(object? sender, EventArgs e)
    {
#if ANDROID

        try
        {
            var context = Android.App.Application.Context;

            Intent? launchIntent =
                context.PackageManager?
                .GetLaunchIntentForPackage("com.rescuar.ar");

            if (launchIntent != null)
            {
                launchIntent.AddFlags(ActivityFlags.NewTask);
                context.StartActivity(launchIntent);
            }
            else
            {
                await DisplayAlertAsync(
                    "Error",
                    "Unity AR app is not installed.",
                    "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Error",
                ex.Message,
                "OK");
        }

#endif
    }
}
