namespace RescuAR.App.Views;

public partial class DashboardPage : ContentPage
{
    public DashboardPage()
    {
        InitializeComponent();
    }

    private async void OnStartARClicked(object sender, EventArgs e)
    {
        await DisplayAlertAsync(
            "AR",
            "Unity AR module will launch here.",
            "OK");
    }
}
