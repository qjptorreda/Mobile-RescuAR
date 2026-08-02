using Microsoft.Extensions.Logging;
using RescuAR.App.Views.Authentication;
using RescuAR.App.ViewModels.Authentication;
using RescuAR.App.Services.Authentication;

namespace RescuAR.App
{
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

            // Register Views and ViewModels
            builder.Services.AddTransient<SplashPage>();
            builder.Services.AddTransient<SplashViewModel>();
            builder.Services.AddTransient<OnboardingPage>();
            builder.Services.AddTransient<OnboardingViewModel>();
            builder.Services.AddTransient<RegistrationPage>();
            builder.Services.AddTransient<RegistrationViewModel>();
            builder.Services.AddTransient<RegistrationSuccessPage>();
            builder.Services.AddTransient<RegistrationSuccessViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<GoogleAuthPage>();
            builder.Services.AddTransient<GoogleAuthViewModel>();

#if ANDROID
            
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
                settings.AllowUniversalAccessFromFileURLs = true;
                settings.AllowFileAccessFromFileURLs = true;
                settings.MixedContentMode = global::Android.Webkit.MixedContentHandling.AlwaysAllow;
            });
#endif

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

#if ANDROID
public class MyWebChromeClient : global::Android.Webkit.WebChromeClient
{
    public override void OnPermissionRequest(global::Android.Webkit.PermissionRequest request)
    {
        request.Grant(request.GetResources());
    }

    public override void OnGeolocationPermissionsShowPrompt(string origin, global::Android.Webkit.GeolocationPermissions.ICallback callback)
    {
        callback.Invoke(origin, true, false);
    }
}
#endif
