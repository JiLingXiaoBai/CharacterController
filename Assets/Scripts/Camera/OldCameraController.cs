using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ARPG.Input;

namespace ARPG.Camera
{
    // todo 加入锁定逻辑，并使用QFramework重构
    public class OldCameraController : MonoBehaviour
    {
        private const float TopClamp = 70.0f;
        private const float BottomClamp = -30.0f;
        private const float Sensitivity = 0.12f;

        private const float CameraThreshold = 0.01f;

        // 俯仰角
        private float _cameraTargetPitch;

        // 偏航角
        private float _cameraTargetYaw;
        
        public Transform cameraTarget;
        

        private void Awake()
        {
            var targetEulerAngles = cameraTarget.rotation.eulerAngles;
            _cameraTargetYaw = targetEulerAngles.y;
            _cameraTargetPitch = targetEulerAngles.x;
        }

        private void LateUpdate()
        {
            var cameraLook = InputMgr.Instance.CameraLook;

            if (cameraLook.sqrMagnitude >= CameraThreshold)
            {
                _cameraTargetPitch -= cameraLook.y * Sensitivity;
                _cameraTargetYaw += cameraLook.x * Sensitivity;
            }

            _cameraTargetPitch = ClampAngle(_cameraTargetPitch, BottomClamp, TopClamp);
            _cameraTargetYaw = ClampAngle(_cameraTargetYaw, float.MinValue, float.MaxValue);
            cameraTarget.rotation = Quaternion.Euler(_cameraTargetPitch, _cameraTargetYaw, 0f);

        }

        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f) angle += 360f;
            if (angle > 360f) angle -= 360f;
            return Mathf.Clamp(angle, min, max);
        }


        
    }
}