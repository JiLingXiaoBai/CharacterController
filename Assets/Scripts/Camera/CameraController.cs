using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTarget;
    
    private void Awake()
    {
        //todo Test to delete
#if UNITY_EDITOR
        InputMgr.Instance.Init();
#endif
        CameraMgr.Instance.Init(cameraTarget.rotation.eulerAngles);
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        var cameraLook = InputMgr.Instance.cameraLook;
        var cameraRotation = CameraMgr.Instance.CameraRotationViaInput(cameraLook);
        cameraTarget.rotation = cameraRotation;
    }
}