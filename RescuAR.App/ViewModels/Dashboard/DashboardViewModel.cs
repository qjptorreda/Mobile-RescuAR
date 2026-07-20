using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;

namespace RescuAR.App.ViewModels.Dashboard;

public partial class DashboardViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string UserName { get; set; } = "Aubrey";

    [ObservableProperty]
    public partial string Greeting { get; set; } = "Be safe out there,";

    public DashboardViewModel()
    {
        // Load dynamically saved User Name from Preferences or default to Aubrey
        UserName = Preferences.Get("UserName", "Aubrey");
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
}
