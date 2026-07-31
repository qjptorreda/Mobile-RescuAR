using Microsoft.Maui.Controls;
using RescuAR.App.ViewModels.Dashboard;

namespace RescuAR.App.Views.Dashboard;

public partial class DashboardPage : ContentView
{
    public DashboardPage()
    {
        InitializeComponent();
        BindingContext = new DashboardViewModel();
    }
}
