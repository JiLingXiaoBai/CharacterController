using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTarget;

    private void Awake()
    {
        CameraMgr.Instance.Init(cameraTarget.rotation.eulerAngles);
    }

    private void LateUpdate()
    {
        var cameraLook = InputMgr.Instance.CameraLook;
        var cameraRotation = CameraMgr.Instance.CameraRotateViaInput(cameraLook);
        cameraTarget.rotation = cameraRotation;
    }
}