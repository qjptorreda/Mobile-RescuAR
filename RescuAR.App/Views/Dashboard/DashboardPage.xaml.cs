using Microsoft.Maui.Controls;
using RescuAR.App.ViewModels.Dashboard;

namespace RescuAR.App.Views.Dashboard;

public partial class DashboardPage : ContentPage
{
    public DashboardPage()
    {
        InitializeComponent();
        BindingContext = new DashboardViewModel();
    }
}
