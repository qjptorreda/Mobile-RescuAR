using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RescuAR.App.Models
{
    public partial class CommunityReport : ObservableObject
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [ObservableProperty]
        private string title = string.Empty;

        [ObservableProperty]
        private string description = string.Empty;

        [ObservableProperty]
        private string address = string.Empty;

        [ObservableProperty]
        private double latitude;

        [ObservableProperty]
        private double longitude;

        [ObservableProperty]
        private string distanceText = "320 meters away";

        [ObservableProperty]
        private string postedBy = "Aubrey T.";

        [ObservableProperty]
        private string category = "Flood Warning"; // Flood Warning, Road Hazard, Power Outage, Rescue Request, General Alert

        [ObservableProperty]
        private string authorAvatar = "";

        [ObservableProperty]
        private DateTime postedAt = DateTime.Now;

        [ObservableProperty]
        private string mediaUrl = string.Empty;

        [ObservableProperty]
        private string mediaType = "Image"; // "Image" or "Video"

        [ObservableProperty]
        private bool hasMedia;

        [ObservableProperty]
        private bool allowComments = true;

        [ObservableProperty]
        private int likeCount;

        [ObservableProperty]
        private bool isLikedByCurrentUser;

        [ObservableProperty]
        private ObservableCollection<CommunityComment> comments = new();

        public int CommentCount => Comments.Count;

        public string PostedAtText => PostedAt.ToString("MMM d, yyyy • h:mm tt");

        public string AuthorInitials
        {
            get
            {
                if (string.IsNullOrWhiteSpace(PostedBy)) return "AT";
                var parts = PostedBy.Trim().Split(' ');
                if (parts.Length == 1) return parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpper();
                return $"{parts[0][0]}{parts[parts.Length - 1][0]}".ToUpper();
            }
        }

        public string CategoryBadgeColor => Category switch
        {
            "Flood Warning" => "#EFF6FF",
            "Rescue Request" => "#FEF2F2",
            "Road Hazard" => "#FFFBEB",
            "Power Outage" => "#F3E8FF",
            _ => "#F1F5F9"
        };

        public string CategoryTextColor => Category switch
        {
            "Flood Warning" => "#1E40AF",
            "Rescue Request" => "#DC2626",
            "Road Hazard" => "#B45309",
            "Power Outage" => "#6B21A8",
            _ => "#475569"
        };

        public string CategoryIcon => Category switch
        {
            "Flood Warning" => "M12,3.25C12,3.25 6,10 6,14A6,6 0 0,0 12,20A6,6 0 0,0 18,14C18,10 12,3.25 12,3.25Z",
            "Rescue Request" => "M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2M11,7H13V13H11V7M11,15H13V17H11V15Z",
            "Road Hazard" => "M12,2L1,21H23L12,2M12,6L19.8,20H4.2L12,6M11,10V14H13V10H11M11,16V18H13V16H11Z",
            "Power Outage" => "M7,2H17A1,1 0 0,1 18,3V6A3,3 0 0,1 15,9V21A1,1 0 0,1 14,22H10A1,1 0 0,1 9,21V9A3,3 0 0,1 6,6V3A1,1 0 0,1 7,2Z",
            _ => "M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2Z"
        };

        public void NotifyCommentsChanged()
        {
            OnPropertyChanged(nameof(CommentCount));
        }
    }
}
