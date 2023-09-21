using JLXB.Framework.Timer;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTarget;
    
    private void Start()
    {
        CameraMgr.Instance.Init(cameraTarget.rotation.eulerAngles);
    }
    
    private void LateUpdate()
    {
        var cameraLook = InputMgr.Instance.CameraLook;
        var cameraRotation = CameraMgr.Instance.CameraRotationViaInput(cameraLook);
        cameraTarget.rotation = cameraRotation;
    }
}