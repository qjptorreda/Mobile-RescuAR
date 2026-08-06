using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using RescuAR.App.Services.Authentication;
using RescuAR.App.Services.Dashboard;
using RescuAR.App.Services.Reports;
using RescuAR.App.Services.Unity;
using RescuAR.App.ViewModels.Authentication;
using RescuAR.App.ViewModels.Dashboard;
using RescuAR.App.ViewModels.Map;
using RescuAR.App.ViewModels.Prepare;
using RescuAR.App.ViewModels.Profile;
using RescuAR.App.ViewModels.Reports;
using RescuAR.App.Views.Authentication;
using RescuAR.App.Views.Camera;
using RescuAR.App.Views.Dashboard;
using RescuAR.App.Views.Map;
using RescuAR.App.Views.Prepare;
using RescuAR.App.Views.Profile;
using RescuAR.App.Views.Reports;

namespace RescuAR.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register Services
        builder.Services.AddSingleton<AuthenticationService>();
        builder.Services.AddSingleton<IDashboardDataService, DashboardDataService>();
        builder.Services.AddSingleton<CommunityReportService>();
        builder.Services.AddSingleton<IOsmGeocodingService, OsmGeocodingService>();
        builder.Services.AddSingleton<AdvisoryService>();

        // Register ViewModels & Pages
        builder.Services.AddTransient<SplashViewModel>();
        builder.Services.AddTransient<SplashPage>();
        builder.Services.AddTransient<OnboardingViewModel>();
        builder.Services.AddTransient<OnboardingPage>();
        builder.Services.AddTransient<RegistrationViewModel>();
        builder.Services.AddTransient<RegistrationPage>();
        builder.Services.AddTransient<RegistrationSuccessViewModel>();
        builder.Services.AddTransient<RegistrationSuccessPage>();
        builder.Services.AddTransient<GoogleAuthPage>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<LoginPage>();
        
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<DashboardPage>();
        builder.Services.AddTransient<AreaStatusOverviewViewModel>();
        builder.Services.AddTransient<AreaStatusOverviewPage>();
        builder.Services.AddTransient<AdvisoriesActivePage>();
        builder.Services.AddTransient<CommunityReportsOverviewViewModel>();
        builder.Services.AddTransient<CommunityReportsOverviewPage>();
        builder.Services.AddTransient<DisasterInformationViewModel>();
        builder.Services.AddTransient<DisasterInformationPage>();
        builder.Services.AddTransient<EvacuationOverviewViewModel>();
        builder.Services.AddTransient<EvacuationOverviewPage>();
        builder.Services.AddTransient<PASSOverviewViewModel>();
        builder.Services.AddTransient<PASSOverviewPage>();
        builder.Services.AddTransient<PreparednessOverviewViewModel>();
        builder.Services.AddTransient<PreparednessOverviewPage>();
        builder.Services.AddTransient<QuickActionsViewModel>();
        builder.Services.AddTransient<QuickActionsPage>();
        builder.Services.AddTransient<SafetyCircleOverviewViewModel>();
        builder.Services.AddTransient<SafetyCircleOverviewPage>();
        builder.Services.AddTransient<WeatherInformationViewModel>();
        builder.Services.AddTransient<WeatherInformationPage>();

        builder.Services.AddTransient<MapViewModel>();
        builder.Services.AddTransient<MapPage>();
        builder.Services.AddTransient<CameraPage>();
        builder.Services.AddTransient<ReportsViewModel>();
        builder.Services.AddTransient<ReportsPage>();
        builder.Services.AddTransient<ProfileViewModel>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<ChecklistViewModel>();
        builder.Services.AddTransient<ChecklistPage>();
        builder.Services.AddTransient<PASSViewModel>();
        builder.Services.AddTransient<PASSPage>();
        builder.Services.AddTransient<AssessmentViewModel>();
        builder.Services.AddTransient<AssessmentPage>();
        builder.Services.AddTransient<EvacuationCenterInfoViewModel>();
        builder.Services.AddTransient<EvacuationCenterInfoPage>();
        builder.Services.AddTransient<ReportDetailsPage>();
        builder.Services.AddTransient<ReportDetailsViewModel>();

        // Summary & Advisory Pages
        builder.Services.AddTransient<RescuAR.App.Views.Summary.SummaryPage>();
        builder.Services.AddTransient<RescuAR.App.ViewModels.Summary.SummaryViewModel>();
        builder.Services.AddTransient<RescuAR.App.Views.Reports.AdvisoryFeedPage>();
        builder.Services.AddTransient<RescuAR.App.ViewModels.Reports.AdvisoryFeedViewModel>();

#if ANDROID
        builder.Services.AddSingleton<IUnityService, RescuAR.App.Platforms.Android.Unity.UnityService>();
        
        // Custom WebView mapping to grant camera and geolocation access to the WebView
        Microsoft.Maui.Handlers.WebViewHandler.Mapper.AppendToMapping("WebSettings", (handler, view) =>
        {
            handler.PlatformView.SetWebChromeClient(new MyWebChromeClient());
            
            var settings = handler.PlatformView.Settings;
            settings.JavaScriptEnabled = true;
            settings.DomStorageEnabled = true;
            settings.DatabaseEnabled = true;
            settings.SetGeolocationEnabled(true);
            settings.AllowFileAccess = true;
            settings.AllowContentAccess = true;
        });
#endif

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

#if ANDROID
public class MyWebChromeClient : Android.Webkit.WebChromeClient
{
    public override void OnPermissionRequest(Android.Webkit.PermissionRequest? request)
    {
        if (request != null)
        {
            // Automatically grant all web permissions (Camera, Microphone, Geolocation, etc.)
            request.Grant(request.GetResources());
        }
    }
}
#endif
