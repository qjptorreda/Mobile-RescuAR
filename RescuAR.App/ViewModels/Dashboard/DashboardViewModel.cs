using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace RescuAR.App.ViewModels.Dashboard;

public partial class DashboardViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string UserName { get; set; } = "Aubrey";

    [ObservableProperty]
    public partial string Greeting { get; set; } = "Good day,";

    [ObservableProperty]
    public partial int PreparednessScore { get; set; } = 72;

    [ObservableProperty]
    public partial double ScoreProgress { get; set; } = 0.72;

    [ObservableProperty]
    public partial string PreparednessStatus { get; set; } = "Prepared";

    [ObservableProperty]
    public partial int ActiveAdvisoriesCount { get; set; } = 2;

    [ObservableProperty]
    public partial string WeatherSummary { get; set; } = "28°C • Light Rain";

    [ObservableProperty]
    public partial string FloodRiskLevel { get; set; } = "Moderate Flood Risk";

    public DashboardViewModel()
    {
        RefreshDashboard();
    }

    public void RefreshDashboard()
    {
        UserName = Preferences.Get("UserName", "Aubrey");
        PreparednessScore = Preferences.Get("PASS_Score", 72);
        ScoreProgress = PreparednessScore / 100.0;
        PreparednessStatus = Preferences.Get("PASS_Status", "Prepared");

        int hour = DateTime.Now.Hour;
        if (hour < 12) Greeting = "Good morning,";
        else if (hour < 18) Greeting = "Good afternoon,";
        else Greeting = "Good evening,";
    }

    [RelayCommand]
    private async Task OpenProfileAsync()
    {
        if (Shell.Current != null)
        {
            try
            {
                await Shell.Current.GoToAsync("//ProfilePage");
            }
            catch (Exception)
            {
                string result = await Shell.Current.DisplayPromptAsync(
                    "User Profile",
                    "Update your display name:",
                    initialValue: UserName);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    UserName = result.Trim();
                    Preferences.Set("UserName", UserName);
                }
            }
        }
    }

    [RelayCommand]
    private async Task OpenPASSAsync()
    {
        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync("Prepare/PASS");
        }
    }

    [RelayCommand]
    private async Task OpenChecklistAsync()
    {
        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync("Prepare/Checklist");
        }
    }

    [RelayCommand]
    private async Task OpenEvacuationAsync()
    {
        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync("Prepare/EvacuationCenterInfo");
        }
    }
}
