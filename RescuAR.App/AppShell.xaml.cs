namespace RescuAR.App;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("Prepare/Checklist", typeof(Views.Prepare.ChecklistPage));
        Routing.RegisterRoute("Prepare/PASS", typeof(Views.Prepare.PASSPage));
        Routing.RegisterRoute("Prepare/Assessment", typeof(Views.Prepare.AssessmentPage));
        Routing.RegisterRoute("Prepare/EvacuationCenterInfo", typeof(Views.Prepare.EvacuationCenterInfoPage));
        Routing.RegisterRoute("Prepare/HotlineDirectory", typeof(Views.Prepare.EvacuationCenterInfoPage));
        Routing.RegisterRoute("AdvisoryFeedPage", typeof(Views.Reports.AdvisoryFeedPage));
        Routing.RegisterRoute("Reports/AdvisoryFeed", typeof(Views.Reports.AdvisoryFeedPage));
        Routing.RegisterRoute("ProfilePage", typeof(Views.Profile.ProfilePage));
        Routing.RegisterRoute("Reports/CommunityPosting", typeof(Views.Reports.CommunityPostingPage));
        Routing.RegisterRoute("ReportDetails", typeof(Views.Reports.ReportDetailsPage));
        
        // Summary & Advisory Routes (registered to both Dashboard & Reports/Summary folders)
        Routing.RegisterRoute("SummaryPage", typeof(Views.Dashboard.SummaryPage));
        Routing.RegisterRoute("AreaStatusSummaryPage", typeof(Views.Dashboard.SummaryPage));
        Routing.RegisterRoute("AdvisoryFeedPage", typeof(Views.Dashboard.AdvisoryFeedPage));
        Routing.RegisterRoute("Reports/AdvisoryFeed", typeof(Views.Dashboard.AdvisoryFeedPage));
    }
}
