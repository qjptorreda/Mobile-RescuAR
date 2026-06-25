namespace RescuAR.App.Services.Unity;

public interface IUnityService
{
    void LaunchUnity(
        string destinationName,
        double latitude,
        double longitude);
}
