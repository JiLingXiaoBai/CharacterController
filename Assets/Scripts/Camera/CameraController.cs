using ARPG.Input;
using Cinemachine;
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

    public class CameraController : AbstractController
    {
        private const float TopClamp = 45.0f;
        private const float BottomClamp = -30.0f;
        private const float RotateSpeed = 5f;

        private const float CameraThreshold = 0.01f;

        // 俯仰角
        private float _cameraTargetPitch;

        // 偏航角
        private float _cameraTargetYaw;

        [SerializeField] private CinemachineVirtualCamera defaultCamera;
        [SerializeField] private GameObject lockTarget;
        private GameObject _followRoot;
        private StateMachine<CameraState, string> _cameraStateMachine;

        private void Awake()
        {
            _followRoot = new GameObject
            {
                name = "DefaultCameraFollowRoot",
                transform =
                {
                    parent = defaultCamera.transform.parent,
                    position = transform.position
                }
            };

            defaultCamera.Follow = _followRoot.transform;
        }

        private void Start()
        {
            var model = this.GetModel<ICameraModel>();
            _cameraStateMachine = new StateMachine<CameraState, string>();
            _cameraStateMachine.AddState(CameraState.Follow,
                onEnter: _ => { SwitchCameraState(CameraState.Follow); },
                onLogic: _ => { FollowCameraUpdate(); });
            _cameraStateMachine.AddState(CameraState.LockOn,
                onEnter: _ => { SwitchCameraState(CameraState.LockOn); },
                onLogic: _ => { LockOnCameraUpdate(); });
            _cameraStateMachine.AddState(CameraState.CloseUp,
                onEnter: _ => { SwitchCameraState(CameraState.CloseUp); });
            _cameraStateMachine.SetStartState(CameraState.Follow);
            _cameraStateMachine.AddTwoWayTriggerTransition("CameraStateChange", CameraState.Follow, CameraState.LockOn,
                t => model.IsLockOn.Value);
            _cameraStateMachine.AddTriggerTransition("CameraStateChange", CameraState.Follow, CameraState.CloseUp,
                t => !model.IsLockOn.Value && model.IsClosingUp.Value);
            _cameraStateMachine.AddTriggerTransition("CameraStateChange", CameraState.LockOn, CameraState.CloseUp,
                t => model.IsLockOn.Value && model.IsClosingUp.Value);
            _cameraStateMachine.AddTriggerTransition("CameraStateChange", CameraState.CloseUp, CameraState.Follow,
                t => !model.IsLockOn.Value && !model.IsClosingUp.Value);
            _cameraStateMachine.AddTriggerTransition("CameraStateChange", CameraState.CloseUp, CameraState.LockOn,
                t => model.IsLockOn.Value && !model.IsClosingUp.Value);
            _cameraStateMachine.Init();

            model.IsLockOn.RegisterWithInitValue(newCount => { _cameraStateMachine.Trigger("CameraStateChange"); })
                .UnRegisterWhenGameObjectDestroyed(gameObject);
            model.IsClosingUp.RegisterWithInitValue(newCount => { _cameraStateMachine.Trigger("CameraStateChange"); })
                .UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void Update()
        {
            if (this.GetModel<IInputModel>().LockOn)
            {
                this.GetModel<ICameraModel>().IsLockOn.Value = true;
            }
        }

        private void LateUpdate()
        {
            _followRoot.transform.position = transform.position;
            _cameraStateMachine.OnLogic();
        }

        private void SwitchCameraState(CameraState state)
        {
            switch (state)
            {
                case CameraState.Follow:
                    break;
                case CameraState.LockOn:
                    break;
                case CameraState.CloseUp:
                    break;
                default:
                    break;
            }
        }

        private void FollowCameraUpdate()
        {
            var cameraLook = this.GetModel<IInputModel>().CameraLook;

            if (cameraLook.sqrMagnitude >= CameraThreshold)
            {
                _cameraTargetPitch -= cameraLook.y * RotateSpeed * Time.deltaTime;
                _cameraTargetYaw += cameraLook.x * RotateSpeed * Time.deltaTime;
            }
            _cameraTargetPitch = ClampAngle(_cameraTargetPitch, BottomClamp, TopClamp);
            _followRoot.transform.rotation = Quaternion.Euler(_cameraTargetPitch, _cameraTargetYaw, 0f);
        }

        private void LockOnCameraUpdate()
        {
            var lockDir = lockTarget.transform.position - _followRoot.transform.position;
            var targetRotation = Quaternion.LookRotation(lockDir);
            var eulerAngles = targetRotation.eulerAngles;
            eulerAngles.x = ClampAngle(eulerAngles.x, BottomClamp, TopClamp);
            targetRotation = Quaternion.Euler(eulerAngles);
            _followRoot.transform.rotation =
                Quaternion.RotateTowards(_followRoot.transform.rotation, targetRotation, RotateSpeed);
        }


        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle is < 90f or > 270f)
            {
                if (angle > 180)
                {
                    angle -= 360f;
                }
                if (max > 180) max -= 360f;
                if (min > 180) min -= 360f;
            }
            angle = Mathf.Clamp(angle, min, max);
            if (angle < 0f) angle += 360f;
            return angle;
        }
    }
}