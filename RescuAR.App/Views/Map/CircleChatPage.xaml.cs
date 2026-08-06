using Microsoft.Maui.Controls;

namespace RescuAR.App.Views.Map;

public partial class CircleChatPage : ContentPage
{
    public CircleChatPage()
    {
        InitializeComponent();
    }

    private async void OnBackClicked(object sender, System.EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private void OnSendClicked(object sender, System.EventArgs e)
    {
        // TODO: Send message logic
        if (!string.IsNullOrWhiteSpace(MessageEntry.Text))
        {
            MessageEntry.Text = string.Empty;
        }
    }
}
