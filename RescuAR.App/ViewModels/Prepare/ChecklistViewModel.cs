using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace RescuAR.App.ViewModels.Prepare;

public partial class ChecklistItem : ObservableObject
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsCompleted { get; set; }
}

public partial class ChecklistViewModel : ObservableObject
{
    public ObservableCollection<ChecklistItem> Items { get; } = new();

    [ObservableProperty]
    public partial double ProgressValue { get; set; } = 0.6;

    [ObservableProperty]
    public partial int PercentReady { get; set; } = 60;

    [ObservableProperty]
    public partial string PreparedCountText { get; set; } = "6 of 10 emergency items prepared";

    [ObservableProperty]
    public partial string SelectedTab { get; set; } = "After"; // Default to After matching mockup

    public ObservableCollection<string> CurrentWhatToDo { get; } = new();

    private readonly Dictionary<string, List<string>> _whatToDoData = new()
    {
        { "Before", new List<string> { "Prepare a 3-day supply of food and water", "Secure heavy furniture and fixtures", "Identify safe zones in your home", "Keep emergency hotlines saved" } },
        { "During", new List<string> { "Drop, Cover, and Hold on (for earthquakes)", "Move to higher ground immediately (for floods)", "Turn off main electricity and gas lines", "Stay tuned to official radio advisories" } },
        { "After", new List<string> { "Check for Injuries", "Avoid damaged structures", "Await official clearance before returning home", "Monitor updates for secondary tasks" } }
    };

    public ChecklistViewModel()
    {
        LoadItems();
        UpdateProgress();
        SelectTab("After");
    }

    private void LoadItems()
    {
        Items.Add(new ChecklistItem { Title = "Drinking Water (3-Day Supply)", Description = "Store at least 3 days' worth of clean drinking water.", IsCompleted = true });
        Items.Add(new ChecklistItem { Title = "Extra Clothing", Description = "Pack weather-appropriate clothing and sturdy shoes.", IsCompleted = true });
        Items.Add(new ChecklistItem { Title = "Non-perishable Food", Description = "Keep ready-to-eat food that requires no refrigeration.", IsCompleted = false });
        Items.Add(new ChecklistItem { Title = "Flashlight (with Extra Batteries)", Description = "Ensure you have a working flashlight and spares.", IsCompleted = true });
        Items.Add(new ChecklistItem { Title = "Portable Radio", Description = "Use a battery-powered radio to receive official updates.", IsCompleted = false });
        Items.Add(new ChecklistItem { Title = "First Aid Kit", Description = "Prepare essential medical supplies to treat injuries.", IsCompleted = true });
        Items.Add(new ChecklistItem { Title = "Whistle", Description = "Use a whistle to signal for help if trapped.", IsCompleted = true });
        Items.Add(new ChecklistItem { Title = "Power Bank", Description = "Keep a fully charged power bank to maintain communication.", IsCompleted = true });
        Items.Add(new ChecklistItem { Title = "Important Documents (Waterproof)", Description = "Store identification, medical records, and certificates.", IsCompleted = false });
        Items.Add(new ChecklistItem { Title = "Prescription Medications", Description = "Maintain at least a 3-day supply of required meds.", IsCompleted = false });
    }

    [RelayCommand]
    private void ToggleItem(ChecklistItem item)
    {
        if (item == null) return;
        item.IsCompleted = !item.IsCompleted;
        UpdateProgress();
    }

    private void UpdateProgress()
    {
        int completedCount = Items.Count(x => x.IsCompleted);
        int totalCount = Items.Count;
        if (totalCount > 0)
        {
            ProgressValue = (double)completedCount / totalCount;
            PercentReady = (int)(ProgressValue * 100);
            PreparedCountText = $"{completedCount} of {totalCount} emergency items prepared";
        }
    }

    [RelayCommand]
    private void SelectTab(string tabName)
    {
        SelectedTab = tabName;
        CurrentWhatToDo.Clear();
        if (_whatToDoData.TryGetValue(tabName, out var list))
        {
            foreach (var item in list)
            {
                CurrentWhatToDo.Add(item);
            }
        }
    }

    [RelayCommand]
    private async Task NavigateToEvacuationCentersAsync()
    {
        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync("Prepare/EvacuationCenterInfo");
        }
    }
}
