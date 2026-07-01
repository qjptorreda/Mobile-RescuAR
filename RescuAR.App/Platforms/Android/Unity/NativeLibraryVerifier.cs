using Android.Util;
using Java.Lang;

namespace RescuAR.App.Platforms.Android.Unity;

public static class NativeLibraryVerifier
{
    public static void Verify()
    {
        TryLoad("unity");
        TryLoad("main");
        TryLoad("il2cpp");
    }

    private static void TryLoad(
        string libraryName)
    {
        try
        {
            Runtime.GetRuntime();

            JavaSystem.LoadLibrary(
                libraryName);

            Log.Debug(
                "RESCUAR_UAAL",
                $"Loaded {libraryName}");
        }
        catch (System.Exception ex)
        {
            Log.Error(
                "RESCUAR_UAAL",
                $"Failed {libraryName}: {ex}");
        }
    }
}
