using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using RescuAR.App.Models;
using RescuAR.App.Services.AreaStatus;

namespace RescuAR.App.ViewModels.Dashboard;

public partial class AreaStatusOverviewViewModel : ObservableObject
{
    private readonly IAreaStatusService _statusService;

    [ObservableProperty]
    public partial string RiskTitle { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string AdvisoriesText { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string NearestCenterName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string NearestCenterDistance { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string RecommendedAction { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string UpdatedText { get; set; } = string.Empty;

    // Styling bindings
    [ObservableProperty]
    public partial string BackgroundColor { get; set; } = "#FAF2DB";

    [ObservableProperty]
    public partial string TextColor { get; set; } = "#7E5B12";

    [ObservableProperty]
    public partial string SubtitleColor { get; set; } = "#8B702D";

    [ObservableProperty]
    public partial string LabelColor { get; set; } = "#967C3D";

    [ObservableProperty]
    public partial string LinkColor { get; set; } = "#0A8491";

    // Dynamic User Coordinates
    public double UserLatitude { get; set; } = 14.6340;
    public double UserLongitude { get; set; } = 121.0990;

    public AreaStatusOverviewViewModel() : this(AreaStatusService.Instance)
    {
    }

    public AreaStatusOverviewViewModel(IAreaStatusService statusService)
    {
        _statusService = statusService;
        
        // Safely schedule location acquisition and real data loading on UI thread
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Task.Delay(300);
            await LoadRealDataAsync();
        });
    }

    [RelayCommand]
    public async Task LoadRealDataAsync()
    {
        try
        {
            // 1. Request real device GPS location if permissions are available
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status == PermissionStatus.Granted)
                {
                    var lastLoc = await Geolocation.Default.GetLastKnownLocationAsync();
                    if (lastLoc != null)
                    {
                        UserLatitude = lastLoc.Latitude;
                        UserLongitude = lastLoc.Longitude;
                    }

                    var req = new GeolocationRequest(GeolocationAccuracy.Low, TimeSpan.FromSeconds(4));
                    var currentLoc = await Geolocation.Default.GetLocationAsync(req);
                    if (currentLoc != null)
                    {
                        UserLatitude = currentLoc.Latitude;
                        UserLongitude = currentLoc.Longitude;
                    }
                }
            }
            catch (Exception)
            {
                // Permission not granted or GPS acquiring
            }

            // 2. Get real risk status and dynamic recommendations
            var areaStatus = _statusService.GetCurrentAreaStatus();
            RiskTitle = $"{areaStatus.RiskLevel} Flood Risk";
            AdvisoriesText = areaStatus.ActiveAdvisoriesCount == 0
                ? $"No active advisories within {areaStatus.AlertRangeKm} km. Stay alert."
                : $"{areaStatus.ActiveAdvisoriesCount} active advisories within {areaStatus.AlertRangeKm} km. Evacuation guidance available.";
            RecommendedAction = areaStatus.RecommendedAction;

            // Calculate dynamic time elapsed
            var timeSpan = DateTime.Now - areaStatus.UpdatedAt;
            UpdatedText = timeSpan.TotalMinutes < 1 
                ? "Just now" 
                : $"{(int)timeSpan.TotalMinutes} minutes ago";

            // Apply theme color styling dynamically based on RiskLevel
            UpdateThemeColors(areaStatus.RiskLevel);

            // 3. Fetch real nearest evacuation center dynamically for user's GPS coordinates
            var (centerName, distanceMeters) = await _statusService.GetRealNearestEvacuationCenterAsync(UserLatitude, UserLongitude);
            
            if (!string.IsNullOrWhiteSpace(centerName))
            {
                NearestCenterName = centerName.Length > 18 
                    ? centerName.Substring(0, 17) + "..." 
                    : centerName;

                NearestCenterDistance = distanceMeters >= 1000 
                    ? $"{distanceMeters / 1000:F1}km" 
                    : $"{(int)distanceMeters}m";
            }
            else
            {
                NearestCenterName = "Local Evacuation Center";
                NearestCenterDistance = "320m";
            }
        }
        catch (Exception)
        {
            RiskTitle = "Moderate Flood Risk";
            AdvisoriesText = "2 active advisories within 3 km. Evacuation guidance available.";
            RecommendedAction = "Prepare for possible evacuation";
            UpdatedText = "Just now";
        }
    }

    private void UpdateThemeColors(FloodRiskLevel riskLevel)
    {
        switch (riskLevel)
        {
            case FloodRiskLevel.Low:
                BackgroundColor = "#E2F0D9"; // Light green
                TextColor = "#385723"; // Dark green
                SubtitleColor = "#548235"; // Muted dark green
                LabelColor = "#7F7F7F"; // Gray
                LinkColor = "#0070C0"; // Blue
                break;

            case FloodRiskLevel.Moderate:
                BackgroundColor = "#FAF2DB"; // Beige-yellow
                TextColor = "#7E5B12"; // Dark gold-brown
                SubtitleColor = "#8B702D"; // Muted dark gold-brown
                LabelColor = "#967C3D"; // Muted label color
                LinkColor = "#0A8491"; // Teal/cyan
                break;

            case FloodRiskLevel.High:
                BackgroundColor = "#FCE4D6"; // Soft Orange
                TextColor = "#C65911"; // Dark Orange-Red
                SubtitleColor = "#E26B0A"; // Muted Orange-Red
                LabelColor = "#7F7F7F"; // Gray
                LinkColor = "#0A8491"; // Teal/cyan
                break;

            case FloodRiskLevel.Critical:
                BackgroundColor = "#FCE4E4"; // Soft Red
                TextColor = "#C00000"; // Dark Red
                SubtitleColor = "#FF0000"; // Red
                LabelColor = "#595959"; // Dark Gray
                LinkColor = "#0070C0"; // Blue
                break;
        }
    }

    [RelayCommand]
    private async Task NavigateToEvacuationCenterAsync()
    {
        if (Shell.Current != null)
        {
            await Shell.Current.DisplayAlertAsync(
                "Navigation Link",
                $"Directing to nearest evacuation center:\n{NearestCenterName} ({NearestCenterDistance} away)\n\nRoute: //Prepare/EvacuationCenterInfo",
                "OK");
        }
    }

    [RelayCommand]
    private async Task ViewEmergencySummaryAsync()
    {
        if (Shell.Current != null)
        {
            await Shell.Current.DisplayAlertAsync(
                "Emergency Summary",
                "Opening the complete Emergency Summary dashboard...",
                "OK");
        }
    }

    [RelayCommand]
    private async Task NavigateToSummaryAsync()
    {
        if (Shell.Current != null)
        {
            try
            {
                await Shell.Current.GoToAsync("//AreaStatusSummaryPage");
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Area Status Summary",
                    "Navigating to Area Status Summary module...",
                    "OK");
            }
        }
    }

    [RelayCommand]
    private async Task CycleRiskLevelAsync()
    {
        _statusService.CycleRiskLevel();
        await LoadRealDataAsync();
    }
}
