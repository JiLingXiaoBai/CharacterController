using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    public Transform cameraTarget;
    public float topClamp = 70.0f;
    public float bottomClamp = -30.0f;
    public float sensitivity = 0.12f;
    // 俯仰角
    private float _cameraTargetPitch;
    // 偏航角
    private float _cameraTargetYaw;
    private const float CameraThreshold = 0.01f;
    private Vector2 _cameraDir;

    private void Awake()
    {
        //todo Test to delete
#if UNITY_EDITOR
        InputMgr.Instance.Init();
#endif
        var cameraTargetAngles = cameraTarget.rotation.eulerAngles;
        _cameraTargetPitch = cameraTargetAngles.x;
        _cameraTargetYaw = cameraTargetAngles.y;
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        _cameraDir = InputMgr.Instance.cameraLook;
        if (_cameraDir.sqrMagnitude >= CameraThreshold)
        {
            _cameraTargetPitch -= _cameraDir.y * sensitivity;
            _cameraTargetYaw += _cameraDir.x * sensitivity;
        }
        _cameraTargetPitch = ClampAngle(_cameraTargetPitch, bottomClamp, topClamp);
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