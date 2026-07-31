using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace RescuAR.App.ViewModels.Prepare;

public class AssessmentHistoryItem
{
    public string Date { get; set; } = string.Empty;
    public string ScoreText { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StatusColor { get; set; } = "#22C55E";
}

public partial class PASSViewModel : ObservableObject
{
    [ObservableProperty]
    public partial bool IsModalVisible { get; set; } = false; // Set to false by default, can be toggled by user

    [ObservableProperty]
    public partial bool IsEmergencySuppliesExpanded { get; set; } = true;

    [ObservableProperty]
    public partial bool IsEvacuationReadinessExpanded { get; set; } = true;

    [ObservableProperty]
    public partial bool IsEmergencyCommunicationExpanded { get; set; } = false;

    [ObservableProperty]
    public partial bool IsHouseholdPreparednessExpanded { get; set; } = false;

    public List<AssessmentHistoryItem> History { get; } = new();

    public PASSViewModel()
    {
        History.Add(new AssessmentHistoryItem { Date = "June 03, 2026 (72%)", ScoreText = "72%", Status = "Prepared", StatusColor = "#22C55E" });
        History.Add(new AssessmentHistoryItem { Date = "May 16, 2026 (65%)", ScoreText = "65%", Status = "Partially Prepared", StatusColor = "#E2B93B" });
        History.Add(new AssessmentHistoryItem { Date = "April 21, 2026 (58%)", ScoreText = "58%", Status = "Partially Prepared", StatusColor = "#E2B93B" });
    }

    [RelayCommand]
    private void ToggleModal()
    {
        IsModalVisible = !IsModalVisible;
    }

    [RelayCommand]
    private async Task StartAssessmentAsync()
    {
        IsModalVisible = false;
        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync("Prepare/Assessment");
        }
    }

    [RelayCommand]
    private void ToggleEmergencySupplies()
    {
        IsEmergencySuppliesExpanded = !IsEmergencySuppliesExpanded;
    }

    [RelayCommand]
    private void ToggleEvacuationReadiness()
    {
        IsEvacuationReadinessExpanded = !IsEvacuationReadinessExpanded;
    }

    [RelayCommand]
    private void ToggleEmergencyCommunication()
    {
        IsEmergencyCommunicationExpanded = !IsEmergencyCommunicationExpanded;
    }

    [RelayCommand]
    private void ToggleHouseholdPreparedness()
    {
        IsHouseholdPreparednessExpanded = !IsHouseholdPreparednessExpanded;
    }

    [RelayCommand]
    private async Task NavigateToChecklistAsync()
    {
        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync("Prepare/Checklist");
        }
    }

    [RelayCommand]
    private async Task NavigateToEvacuationAsync()
    {
        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync("Prepare/EvacuationCenterInfo");
        }
    }

    [RelayCommand]
    private async Task StartARRouteAsync()
    {
        if (Shell.Current != null)
        {
            // Shell tab navigation to Camera
            await Shell.Current.GoToAsync("//Camera");
        }
    }
}
