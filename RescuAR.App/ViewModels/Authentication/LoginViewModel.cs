using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using RescuAR.App.Views.Authentication;
using RescuAR.App.Services.Authentication;

namespace RescuAR.App.ViewModels.Authentication
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AuthenticationService _authService;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private bool _isPasswordVisible = false;

        [ObservableProperty]
        private bool _isLoading = false;

        public bool IsNotLoading => !IsLoading;

        partial void OnIsLoadingChanged(bool value)
        {
            OnPropertyChanged(nameof(IsNotLoading));
        }

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        public bool IsPasswordHidden => !IsPasswordVisible;

        public string PasswordToggleIcon => IsPasswordVisible ? "👁" : "🙈";

        partial void OnIsPasswordVisibleChanged(bool value)
        {
            OnPropertyChanged(nameof(IsPasswordHidden));
            OnPropertyChanged(nameof(PasswordToggleIcon));
        }

        public LoginViewModel(IServiceProvider serviceProvider, AuthenticationService authService)
        {
            _serviceProvider = serviceProvider;
            _authService = authService;
        }

        [RelayCommand]
        private void TogglePasswordVisibility()
        {
            IsPasswordVisible = !IsPasswordVisible;
        }

        [RelayCommand]
        private async Task Login()
        {
            if (IsLoading) return;

            ErrorMessage = string.Empty;
            OnPropertyChanged(nameof(HasError));

            // Enforce registration restriction
            bool hasSignedUp = Preferences.Default.Get("HasSignedUp", false);
            if (!hasSignedUp)
            {
                ErrorMessage = "You must sign up first before logging in.";
                OnPropertyChanged(nameof(HasError));
                return;
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = "Email Address is required.";
                OnPropertyChanged(nameof(HasError));
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Password is required.";
                OnPropertyChanged(nameof(HasError));
                return;
            }

            IsLoading = true;
            try
            {
                // Authenticate with email/password using Supabase
                await _authService.SignInWithEmailAsync(Email.Trim(), Password);

                // Navigate to Dashboard
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (Application.Current != null)
                    {
                        Preferences.Default.Set("IsLoggedIn", true);
                        Application.Current.MainPage = new AppShell();
                    }
                });
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message ?? "Failed to log in. Please check your credentials.";
                OnPropertyChanged(nameof(HasError));
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void GoogleSignIn()
        {
            // Enforce registration restriction
            bool hasSignedUp = Preferences.Default.Get("HasSignedUp", false);
            if (!hasSignedUp)
            {
                ErrorMessage = "You must sign up first before logging in.";
                OnPropertyChanged(nameof(HasError));
                return;
            }

            var googleAuthPage = _serviceProvider.GetRequiredService<GoogleAuthPage>();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (Application.Current != null)
                {
                    Application.Current.MainPage = googleAuthPage;
                }
            });
        }

        [RelayCommand]
        private void GoToSignUp()
        {
            var registrationPage = _serviceProvider.GetRequiredService<RegistrationPage>();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (Application.Current != null)
                {
                    Application.Current.MainPage = registrationPage;
                }
            });
        }

        [RelayCommand]
        private void Back()
        {
            var onboardingPage = _serviceProvider.GetRequiredService<OnboardingPage>();
            if (onboardingPage.BindingContext is OnboardingViewModel onboardingVm)
            {
                onboardingVm.SetSlideIndex(3); // Go to Entry Screen
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (Application.Current != null)
                {
                    Application.Current.MainPage = onboardingPage;
                }
            });
        }

        [RelayCommand]
        private async Task ResetPassword()
        {
            // Simple placeholder alert to demonstrate reset password trigger
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert("Reset Password", "A password reset link would be sent to your email address if supported by your project authentication settings.", "OK");
            }
        }
    }
}
