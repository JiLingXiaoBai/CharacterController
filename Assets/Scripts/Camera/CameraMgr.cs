using JLXB.Framework;
using UnityEngine;

public class CameraMgr : Singleton<CameraMgr>
{
    private const float TopClamp = 70.0f;
    private const float BottomClamp = -30.0f;
    private const float Sensitivity = 0.12f;
    private const float CameraThreshold = 0.01f;
    // 俯仰角
    private float _cameraTargetPitch;
    // 偏航角
    private float _cameraTargetYaw;
    
    private CameraMgr()
    {
    }

    public void Init(Vector3 targetAngles)
    {
        _cameraTargetYaw = targetAngles.y;
        _cameraTargetPitch = targetAngles.x;
    }
    
    public Quaternion CameraRotateViaInput(Vector2 cameraLook)
    {
        if (cameraLook.sqrMagnitude >= CameraThreshold)
        {
            _cameraTargetPitch -= cameraLook.y * Sensitivity;
            _cameraTargetYaw += cameraLook.x * Sensitivity;
        }
        _cameraTargetPitch = ClampAngle(_cameraTargetPitch, BottomClamp, TopClamp);
        _cameraTargetYaw = ClampAngle(_cameraTargetYaw, float.MinValue, float.MaxValue);
        return Quaternion.Euler(_cameraTargetPitch, _cameraTargetYaw, 0f);
    }
    
    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}