using UnityEngine;

public class ExitARButton : MonoBehaviour
{
    public void ExitAR()
    {
#if UNITY_ANDROID && !UNITY_EDITOR

        using var unityPlayer =
            new AndroidJavaClass(
                "com.unity3d.player.UnityPlayer");

        using var activity =
            unityPlayer.GetStatic<AndroidJavaObject>(
                "currentActivity");

        activity.Call(
            "finishAndRemoveTask");

        AndroidJavaClass process =
            new AndroidJavaClass(
                "android.os.Process");

        process.CallStatic(
            "killProcess",
            process.CallStatic<int>("myPid"));

#endif
    }
}
