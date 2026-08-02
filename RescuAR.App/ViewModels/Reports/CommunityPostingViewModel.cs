using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;

namespace RescuAR.App.ViewModels.Reports;

public partial class CommunityPostItem : ObservableObject
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string AuthorName { get; set; } = "Community Member";
    public string TimestampText { get; set; } = "Just now";
    public string Location { get; set; } = "Marikina City";
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string HazardSeverity { get; set; } = "Moderate"; // Low, Moderate, High, Severe
    public string ImagePath { get; set; } = string.Empty; // File path, URL or base64 Data-URI
    public bool HasImage => !string.IsNullOrWhiteSpace(ImagePath);

    [ObservableProperty]
    public partial int Upvotes { get; set; } = 3;

    [ObservableProperty]
    public partial bool IsUpvoted { get; set; } = false;

    public string SeverityBg => HazardSeverity switch
    {
        "Severe" => "#FEE2E2",
        "High" => "#FFEDD5",
        "Moderate" => "#FEF3C7",
        _ => "#DCFCE7"
    };

    public string SeverityTextColor => HazardSeverity switch
    {
        "Severe" => "#DC2626",
        "High" => "#EA580C",
        "Moderate" => "#D97706",
        _ => "#16A34A"
    };
}

public partial class CommunityPostingViewModel : ObservableObject
{
    public ObservableCollection<CommunityPostItem> Posts { get; } = new();

    [ObservableProperty]
    public partial string NewPostTitle { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string NewPostContent { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string NewPostLocation { get; set; } = "Malanday, Marikina";

    [ObservableProperty]
    public partial string SelectedSeverity { get; set; } = "Moderate";

    [ObservableProperty]
    public partial string AttachedImagePath { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool HasAttachedImage { get; set; } = false;

    [ObservableProperty]
    public partial bool IsCreatingPost { get; set; } = false;

    public CommunityPostingViewModel()
    {
        LoadInitialPosts();
    }

    private void LoadInitialPosts()
    {
        Posts.Clear();
        Posts.Add(new CommunityPostItem
        {
            AuthorName = "Captain Juan Cruz",
            TimestampText = "10 mins ago",
            Location = "Tumana Bridge, Marikina",
            Title = "Water Level Alert Level 2 Reached",
            Content = "Marikina River water level reached 16 meters. Residents near low-lying areas are advised to prepare Go-Bags for pre-emptive evacuation.",
            HazardSeverity = "High",
            ImagePath = "onboarding_flood.jpg",
            Upvotes = 24
        });

        Posts.Add(new CommunityPostItem
        {
            AuthorName = "Elena Santos",
            TimestampText = "35 mins ago",
            Location = "Malanday Elementary School",
            Title = "Evacuation Center Food Packs Arrived",
            Content = "LGU relief teams have delivered hot meals and clean drinking water to Malanday Elementary evacuees.",
            HazardSeverity = "Low",
            ImagePath = "onboarding_flag.jpg",
            Upvotes = 15
        });

        Posts.Add(new CommunityPostItem
        {
            AuthorName = "Mark Tan",
            TimestampText = "1 hour ago",
            Location = "Concepcion Uno",
            Title = "Road Obstruction Cleared",
            Content = "Fallen tree branch on J.P. Rizal St. has been cleared by BFP personnel. Traffic is moving smoothly.",
            HazardSeverity = "Moderate",
            ImagePath = "onboarding_phone.jpg",
            Upvotes = 8
        });
    }

    [RelayCommand]
    private async Task PickImageAsync()
    {
        try
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Select Hazard / Report Photo"
            });

            if (result != null)
            {
                AttachedImagePath = result.FullPath;
                HasAttachedImage = true;
            }
        }
        catch (Exception ex)
        {
            if (Shell.Current != null)
            {
                await Shell.Current.DisplayAlert("Image Selection", $"Could not select image: {ex.Message}", "OK");
            }
        }
    }

    [RelayCommand]
    private async Task CapturePhotoAsync()
    {
        try
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                var photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo != null)
                {
                    AttachedImagePath = photo.FullPath;
                    HasAttachedImage = true;
                }
            }
            else
            {
                await PickImageAsync();
            }
        }
        catch (Exception ex)
        {
            if (Shell.Current != null)
            {
                await Shell.Current.DisplayAlert("Camera Error", $"Could not capture photo: {ex.Message}", "OK");
            }
        }
    }

    [RelayCommand]
    private void RemoveAttachedImage()
    {
        AttachedImagePath = string.Empty;
        HasAttachedImage = false;
    }

    [RelayCommand]
    private void SelectSeverity(string severity)
    {
        SelectedSeverity = severity;
    }

    [RelayCommand]
    private async Task CreatePostAsync()
    {
        if (string.IsNullOrWhiteSpace(NewPostTitle) || string.IsNullOrWhiteSpace(NewPostContent))
        {
            if (Shell.Current != null)
            {
                await Shell.Current.DisplayAlert("Validation", "Please enter both a title and description for your community report.", "OK");
            }
            return;
        }

        string user = Preferences.Get("UserName", "Aubrey");

        var newPost = new CommunityPostItem
        {
            AuthorName = user,
            TimestampText = "Just now",
            Location = string.IsNullOrWhiteSpace(NewPostLocation) ? "Marikina City" : NewPostLocation.Trim(),
            Title = NewPostTitle.Trim(),
            Content = NewPostContent.Trim(),
            HazardSeverity = SelectedSeverity,
            ImagePath = AttachedImagePath,
            Upvotes = 1
        };

        // Insert at top of real-time feed
        Posts.Insert(0, newPost);

        // Reset form
        NewPostTitle = string.Empty;
        NewPostContent = string.Empty;
        AttachedImagePath = string.Empty;
        HasAttachedImage = false;
        IsCreatingPost = false;

        if (Shell.Current != null)
        {
            await Shell.Current.DisplayAlert("Real-Time Broadcast", "Your community alert has been published to the real-time feed!", "OK");
        }
    }

    [RelayCommand]
    private void ToggleUpvote(CommunityPostItem post)
    {
        if (post == null) return;
        post.IsUpvoted = !post.IsUpvoted;
        post.Upvotes += post.IsUpvoted ? 1 : -1;
    }

    [RelayCommand]
    private void ToggleCreatePostModal()
    {
        IsCreatingPost = !IsCreatingPost;
    }
}
