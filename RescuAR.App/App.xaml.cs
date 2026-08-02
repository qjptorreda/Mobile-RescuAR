using RescuAR.App.Views.Authentication;

namespace RescuAR.App;

public partial class App : Application
{
    public App(OnboardingPage onboardingPage)
    {
        InitializeComponent();

        bool isLoggedIn = Preferences.Default.Get("IsLoggedIn", false);
        if (isLoggedIn)
        {
            MainPage = new AppShell();
        }
        else
        {
            MainPage = onboardingPage;
        }
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(MainPage ?? new AppShell());
    }
}
