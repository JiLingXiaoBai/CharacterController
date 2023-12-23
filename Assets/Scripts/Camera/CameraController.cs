using ARPG.Input;
using Cinemachine;
using QFramework;
using UnityEngine;
using UnityHFSM;
namespace ARPG.Camera
{
    public enum CameraState
    {
        Follow,
        LockOn,
        CloseUp,
    }

    public class CameraController : MonoBehaviour, IController
    {
        private const float TopClamp = 70.0f;
        private const float BottomClamp = -30.0f;
        private const float Sensitivity = 0.12f;

        private const float CameraThreshold = 0.01f;

        // 俯仰角
        private float _cameraTargetPitch;

        // 偏航角
        private float _cameraTargetYaw;


        [SerializeField] private CinemachineVirtualCamera followCamera;

        [SerializeField] private CinemachineVirtualCamera lockOnCamera;

        [SerializeField] private CinemachineVirtualCamera closeUpCamera;

        public IArchitecture GetArchitecture()
        {
            return CharacterArchitecture.Interface;
        }

        private ICameraModel _model;

        private StateMachine<CameraState, string> _cameraStateMachine;

        private void Awake()
        {
            var targetEulerAngles = followCamera.Follow.rotation.eulerAngles;
            _cameraTargetYaw = targetEulerAngles.y;
            _cameraTargetPitch = targetEulerAngles.x;
        }

        private void Start()
        {
            _model = this.GetModel<ICameraModel>();
            _cameraStateMachine = new StateMachine<CameraState, string>();
            _cameraStateMachine.AddState(CameraState.Follow,
                onEnter: state => { SwitchCameraState(CameraState.Follow); },
                onLogic: state => { FollowCameraUpdate(); });
            _cameraStateMachine.AddState(CameraState.LockOn,
                onEnter: state => { SwitchCameraState(CameraState.LockOn); });
            _cameraStateMachine.AddState(CameraState.CloseUp,
                onEnter: state => { SwitchCameraState(CameraState.CloseUp); });
            _cameraStateMachine.SetStartState(CameraState.Follow);
            _cameraStateMachine.AddTwoWayTriggerTransition("CameraStateChange", CameraState.Follow, CameraState.LockOn,
                t => _model.IsLockOn.Value);
            _cameraStateMachine.AddTriggerTransition("CameraStateChange", CameraState.Follow, CameraState.CloseUp,
                t => !_model.IsLockOn.Value && _model.IsClosingUp.Value);
            _cameraStateMachine.AddTriggerTransition("CameraStateChange", CameraState.LockOn, CameraState.CloseUp,
                t => _model.IsLockOn.Value && _model.IsClosingUp.Value);
            _cameraStateMachine.AddTriggerTransition("CameraStateChange", CameraState.CloseUp, CameraState.Follow,
                t => !_model.IsLockOn.Value && !_model.IsClosingUp.Value);
            _cameraStateMachine.AddTriggerTransition("CameraStateChange", CameraState.CloseUp, CameraState.LockOn,
                t => _model.IsLockOn.Value && !_model.IsClosingUp.Value);
            _cameraStateMachine.Init();

            _model.IsLockOn.RegisterWithInitValue(newCount => { _cameraStateMachine.Trigger("CameraStateChange"); })
                .UnRegisterWhenGameObjectDestroyed(gameObject);
            _model.IsClosingUp.RegisterWithInitValue(newCount => { _cameraStateMachine.Trigger("CameraStateChange"); })
                .UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void SwitchCameraState(CameraState state)
        {
            followCamera.gameObject.SetActive(state == CameraState.Follow);
            lockOnCamera.gameObject.SetActive(state == CameraState.LockOn);
            closeUpCamera.gameObject.SetActive(state == CameraState.CloseUp);
        }

        private void FollowCameraUpdate()
        {
            var cameraLook = InputMgr.Instance.CameraLook;

            if (cameraLook.sqrMagnitude >= CameraThreshold)
            {
                _cameraTargetPitch -= cameraLook.y * Sensitivity;
                _cameraTargetYaw += cameraLook.x * Sensitivity;
            }

            _cameraTargetPitch = ClampAngle(_cameraTargetPitch, BottomClamp, TopClamp);
            _cameraTargetYaw = ClampAngle(_cameraTargetYaw, float.MinValue, float.MaxValue);
            followCamera.Follow.rotation = Quaternion.Euler(_cameraTargetPitch, _cameraTargetYaw, 0f);
        }

        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f) angle += 360f;
            if (angle > 360f) angle -= 360f;
            return Mathf.Clamp(angle, min, max);
        }

        private void LateUpdate()
        {
            _cameraStateMachine.OnLogic();
        }
    }
}