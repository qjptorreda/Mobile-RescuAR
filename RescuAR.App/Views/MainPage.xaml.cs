using RescuAR.App.Models;
using RescuAR.App.Services.Unity;

namespace RescuAR.App.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        EvacuationCenterPicker.ItemsSource =
            EvacuationCenterRepository.GetEvacuationCenters();
    }

    private void LaunchUnityClicked(
        object? sender,
        EventArgs e)
    {
#if ANDROID

        var safeZone =
            EvacuationCenterPicker.SelectedItem as EvacuationCenter;

        if (safeZone == null)
            return;

        var unityService =
            Handler?
                .MauiContext?
                .Services
                .GetService<IUnityService>();

        unityService?.LaunchUnity(
            safeZone.Name,
            safeZone.Latitude,
            safeZone.Longitude);

#endif
    }
}
