using UnityEngine;

using UnityEngine;

public class CameraLockManager : MonoBehaviour
{
    public Camera playerCamera;
    public Transform lockTarget; // Target position/rotation to lock to

    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private Transform originalParent;
    public bool isCameraLocked = false;

    public void CameraToLock()
    {
        if (isCameraLocked || playerCamera == null || lockTarget == null)
            return;

        // Save the original transform data
        originalCameraPosition = playerCamera.transform.position;
        originalCameraRotation = playerCamera.transform.rotation;
        originalParent = playerCamera.transform.parent;

        // Detach and move camera
        playerCamera.transform.SetParent(null);
        playerCamera.transform.position = lockTarget.position;
        playerCamera.transform.rotation = lockTarget.rotation;

        isCameraLocked = true;
    }

    public void CameraToPlayer()
    {
        if (!isCameraLocked || playerCamera == null)
            return;

        // Reattach camera
        playerCamera.transform.SetParent(originalParent);
        playerCamera.transform.position = originalCameraPosition;
        playerCamera.transform.rotation = originalCameraRotation;

        isCameraLocked = false;
    }
}
