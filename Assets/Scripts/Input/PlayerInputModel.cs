using UnityEngine;
using UnityEngine.InputSystem;

namespace ARPG.Input
{
    public class PlayerInputModel : AbstractModel, IInputModel
    {
        private InputController _inputController;

        public Vector2 Movement => _inputController.Gameplay.Movement.ReadValue<Vector2>();

        public Vector2 CameraLook => _inputController.Gameplay.CameraLook.ReadValue<Vector2>();

        public bool Jump => _inputController.Gameplay.Jump.triggered;

        protected override void OnInit()
        {
            _inputController = new InputController();
        }

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
}