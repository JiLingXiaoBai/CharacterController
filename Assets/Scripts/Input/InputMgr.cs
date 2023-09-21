using JLXB.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputMgr : Singleton<InputMgr>
{
    private readonly InputController _inputController;

    private InputMgr()
    {
        _inputController = new InputController();
    }
    
    public Vector2 Movement => _inputController.Gameplay.Movement.ReadValue<Vector2>();
    public bool Jump => _inputController.Gameplay.Jump.WasPressedThisFrame();
    public bool StopJump => _inputController.Gameplay.Jump.WasReleasedThisFrame();
    public Vector2 CameraLook => _inputController.Gameplay.CameraLook.ReadValue<Vector2>();
    
    public void SwitchToDynamicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    public void SwitchToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
    public void EnableGameplayInput() => SwitchActionMap(_inputController.Gameplay, false);
    
    private void SwitchActionMap(InputActionMap actionMap, bool isUIInput)
    {
        _inputController.Disable();
        actionMap.Enable();

        if (isUIInput)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
