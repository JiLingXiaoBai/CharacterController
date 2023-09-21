using JLXB.Framework;
using UnityEngine;

public class InputMgr : Singleton<InputMgr>
{
    private InputController _inputController;
    private InputMgr(){}

    public void Init()
    {
        _inputController ??= new InputController();
        _inputController.Enable();
        _inputController.PlayerNormal.Enable();
    }
    
    public Vector2 playerMovement => _inputController.PlayerNormal.Movement.ReadValue<Vector2>();
    public bool playerJump => _inputController.PlayerNormal.Jump.WasPressedThisFrame();
    public bool playerStopJump => _inputController.PlayerNormal.Jump.WasReleasedThisFrame();
    public Vector2 cameraLook => _inputController.PlayerNormal.CameraLook.ReadValue<Vector2>();
}
