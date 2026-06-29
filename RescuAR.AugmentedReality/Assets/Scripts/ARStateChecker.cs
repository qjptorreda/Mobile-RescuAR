using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARStateChecker : MonoBehaviour
{
    private ARSessionState previousState;

    private void Update()
    {
        if (ARSession.state != previousState)
        {
            previousState = ARSession.state;

            Debug.Log(
                $"AR STATE CHANGED: {ARSession.state}");
        }
    }
}
