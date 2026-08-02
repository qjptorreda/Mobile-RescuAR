using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace RescuAR.App.ViewModels.Prepare;

public class EmergencyHotlineItem
{
    public string Name { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Icon { get; set; } = "📞";
}

public partial class EvacuationCenterItem : ObservableObject
{
    public string Name { get; set; } = string.Empty;
    public string Distance { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string VerifiedBy { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = "https://images.unsplash.com/photo-1577495508048-b635879837f1?w=500";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CapacityText))]
    [NotifyPropertyChangedFor(nameof(ProgressValue))]
    [NotifyPropertyChangedFor(nameof(StatusPillBg))]
    [NotifyPropertyChangedFor(nameof(StatusPillText))]
    public partial int CurrentOccupancy { get; set; }

    public int MaxCapacity { get; set; } = 500;

    public string Status { get; set; } = "Open";

    public string CapacityText => $"{CurrentOccupancy} / {MaxCapacity} evacuees";

    public double ProgressValue => (double)CurrentOccupancy / MaxCapacity;

    public string StatusPillBg => ProgressValue switch
    {
        >= 0.9 => "#FEE2E2",
        >= 0.7 => "#FEF3C7",
        _ => "#DCFCE7"
    };

    public string StatusPillText => ProgressValue switch
    {
        >= 0.9 => "#DC2626",
        >= 0.7 => "#D97706",
        _ => "#16A34A"
    };

    public string StatusLabel => ProgressValue switch
    {
        >= 0.9 => "Near Full",
        >= 0.7 => "Moderate",
        _ => "Open / Space Available"
    };
}

public partial class EvacuationCenterInfoViewModel : ObservableObject
{
    public ObservableCollection<EmergencyHotlineItem> Hotlines { get; } = new();
    public ObservableCollection<EvacuationCenterItem> EvacuationCenters { get; } = new();

    [ObservableProperty]
    public partial string SelectedFilter { get; set; } = "All";

    public EvacuationCenterInfoViewModel()
    {
        LoadData();
    }

    private void LoadData()
    {
        Hotlines.Clear();
        Hotlines.Add(new EmergencyHotlineItem { Name = "Marikina Rescue 161", Number = "(02) 161", Type = "24/7 Emergency Medical & Rescue", Icon = "🚑" });
        Hotlines.Add(new EmergencyHotlineItem { Name = "Marikina PNP Central", Number = "(02) 8405-0091", Type = "Police Emergency Hotline", Icon = "👮" });
        Hotlines.Add(new EmergencyHotlineItem { Name = "Marikina BFP Fire Dept", Number = "(02) 8646-0427", Type = "Fire & Rescue Brigade", Icon = "🚒" });
        Hotlines.Add(new EmergencyHotlineItem { Name = "Red Cross Marikina", Number = "(02) 8681-3442", Type = "Disaster Relief & Blood Bank", Icon = "🏥" });

        EvacuationCenters.Clear();
        EvacuationCenters.Add(new EvacuationCenterItem 
        { 
            Name = "Malanday Elementary School Gym", 
            Distance = "877 meters away", 
            Address = "48 Visayas St., Malanday, Marikina City", 
            VerifiedBy = "Marikina LGU Relief Command",
            CurrentOccupancy = 210,
            MaxCapacity = 500
        });
        EvacuationCenters.Add(new EvacuationCenterItem 
        { 
            Name = "San Roque High School Evacuation Facility", 
            Distance = "1.2 km away", 
            Address = "Abad Santos St., San Roque, Marikina City", 
            VerifiedBy = "Marikina LGU Relief Command",
            CurrentOccupancy = 380,
            MaxCapacity = 450
        });
        EvacuationCenters.Add(new EvacuationCenterItem 
        { 
            Name = "Concepcion Uno Covered Court", 
            Distance = "2.4 km away", 
            Address = "J.P. Rizal St., Concepcion Uno, Marikina City", 
            VerifiedBy = "Red Cross PH Verified",
            CurrentOccupancy = 120,
            MaxCapacity = 300
        });
    }

    [RelayCommand]
    private async Task DialNumberAsync(string number)
    {
        if (Shell.Current != null)
        {
            await Shell.Current.DisplayAlert("Emergency Hotline", $"Initiating call to {number}...", "Call Now");
        }
    }

    [RelayCommand]
    private async Task CheckInEvacueeAsync(EvacuationCenterItem center)
    {
        if (center == null) return;

        if (center.CurrentOccupancy < center.MaxCapacity)
        {
            center.CurrentOccupancy += 1;
            if (Shell.Current != null)
            {
                await Shell.Current.DisplayAlert("Real-Time Check-In", $"You have successfully checked in at {center.Name}. Real-time capacity updated!", "OK");
            }
        }
        else
        {
            if (Shell.Current != null)
            {
                await Shell.Current.DisplayAlert("Center Full", $"{center.Name} has reached max capacity! Please check nearby centers.", "OK");
            }
        }
    }

    [RelayCommand]
    private async Task NavigateToMapAsync()
    {
        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync("//Map");
        }
    }
}
