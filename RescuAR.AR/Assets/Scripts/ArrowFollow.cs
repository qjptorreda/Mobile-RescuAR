using UnityEngine;

public class ArrowFollow : MonoBehaviour
{
    public Transform cameraTransform;

    void Start()
    {
        transform.position =
            cameraTransform.position +
            cameraTransform.forward * 2f;
    }

    void Update()
    {
        transform.position =
            cameraTransform.position +
            cameraTransform.forward * 2f;

        transform.rotation =
            Quaternion.LookRotation(cameraTransform.forward);
    }
}
