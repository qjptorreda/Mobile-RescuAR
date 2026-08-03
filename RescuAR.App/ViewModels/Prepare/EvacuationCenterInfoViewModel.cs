using System.Collections.Generic;
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
}

public class EvacuationCenterItem
{
    public string Name { get; set; } = string.Empty;
    public string Distance { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string VerifiedBy { get; set; } = string.Empty;
}

public partial class EvacuationCenterInfoViewModel : ObservableObject
{
    public List<EmergencyHotlineItem> Hotlines { get; } = new();
    public List<EvacuationCenterItem> EvacuationCenters { get; } = new();

    public EvacuationCenterInfoViewModel()
    {
        LoadData();
    }

    private void LoadData()
    {
        Hotlines.Add(new EmergencyHotlineItem { Name = "Marikina Rescue 161", Number = "(02) 161", Type = "General Emergency/EMS Hotline" });
        Hotlines.Add(new EmergencyHotlineItem { Name = "Marikina PNP", Number = "(02) 8405-0091", Type = "Police Hotline" });
        Hotlines.Add(new EmergencyHotlineItem { Name = "Marikina BFP", Number = "(02) 8646-0427", Type = "Fire Hotline" });

        EvacuationCenters.Add(new EvacuationCenterItem 
        { 
            Name = "Malanday Elementary School", 
            Distance = "877 meters away", 
            Address = "48 Visayas St., Malanday Marikina City 1805", 
            VerifiedBy = "Marikina LGU" 
        });
        EvacuationCenters.Add(new EvacuationCenterItem 
        { 
            Name = "San Roque High School", 
            Distance = "3200 meters away", 
            Address = "Abad Santos Street, San Roque 1801 Marikina City, Philippines", 
            VerifiedBy = "Marikina LGU" 
        });
    }

    [RelayCommand]
    private async Task DialNumber(string number)
    {
        if (Shell.Current != null)
        {
            await Shell.Current.DisplayAlert("Call Hotline", $"Dialing {number}...", "OK");
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
