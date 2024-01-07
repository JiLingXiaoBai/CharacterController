using System.Collections.Generic;
using ARPG.Actor;
using ARPG.Input;
using Cinemachine;
using UnityEngine;
using UnityHFSM;
using UnityEngine.UI;

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
        private const float RotateSpeed = 3f;
        private const float CameraThreshold = 0.01f;
        private const float ChangeTargetThreshold = 0.5f;
        private const float ChangeTargetInterval = 0.5f;
        private const float LockRange = 10f;

        private readonly Rect _screenRect = new(0, 0, Screen.width, Screen.height);

        private readonly Rect _lockCenterRect = new(0.03125f * Screen.width, 0.03125f * Screen.height,
            0.9375f * Screen.width, 0.9375f * Screen.height);

        private readonly Vector2 _screenCenter = new(Screen.width / 2f, Screen.height / 2f);

        private float _cameraTargetPitch; // 俯仰角

        private float _cameraTargetYaw; // 偏航角

        [SerializeField] private CinemachineVirtualCamera defaultCamera;
        [SerializeField] private Image lockDot;
        [SerializeField] private LayerMask lockObstacleLayer;
        private GameObject _followRoot;
        private StateMachine<CameraState, string> _cameraStateMachine;
        private Collider[] _possibleTargets;
        private UnityEngine.Camera _camera;
        private GetPossibleLockOnTargetsQuery _targetsQuery;
        private Timer _changeTargetTimer;

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
            _camera = UnityEngine.Camera.main ? UnityEngine.Camera.main : new UnityEngine.Camera();
            lockDot.enabled = false;
            defaultCamera.Follow = _followRoot.transform;
            _targetsQuery = new GetPossibleLockOnTargetsQuery(defaultCamera.transform, LockRange,
                LayerMask.GetMask("Enemy"), 10);
            _changeTargetTimer = new Timer();
        }

        private void Start()
        {
            var model = this.GetModel<ICameraModel>();
            model.CameraTrans = _camera.transform;
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

        private bool _changeTargetTimerFlag;

        private void Update()
        {
            CheckInputLockOn();
            CheckInputChangeTarget();
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
                    var rot = _followRoot.transform.rotation;
                    _cameraTargetPitch = rot.eulerAngles.x;
                    _cameraTargetYaw = rot.eulerAngles.y;
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
            _cameraTargetYaw = ClampAngle(_cameraTargetYaw, float.MinValue, float.MaxValue);
            _followRoot.transform.rotation = Quaternion.Euler(_cameraTargetPitch, _cameraTargetYaw, 0f);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void LockOnCameraUpdate()
        {
            var model = this.GetModel<ICameraModel>();
            var lockTarget = model.LockTarget;
            if (lockTarget == null)
            {
                SetUnlock();
                return;
            }

            var lockRootPos = lockTarget.LockRoot.position;
            var lockDir = lockRootPos - _followRoot.transform.position;
            if (lockDir.magnitude > LockRange)
            {
                SetUnlock();
                return;
            }

            var lockDotPos = _camera.WorldToScreenPoint(lockRootPos);
            if (!_screenRect.Contains(lockDotPos))
            {
                SetUnlock();
                return;
            }
            if (Physics.Linecast(_followRoot.transform.position, lockRootPos, lockObstacleLayer))
            {
                SetUnlock();
                return;
            }

            lockDot.enabled = true;
            lockDot.rectTransform.position = lockDotPos;
            var targetRotation = Quaternion.LookRotation(lockDir.normalized);
            var eulerAngles = targetRotation.eulerAngles;
            eulerAngles.x = ClampAngle(eulerAngles.x, BottomClamp, TopClamp);
            targetRotation = Quaternion.Euler(eulerAngles);
            _followRoot.transform.rotation =
                Quaternion.RotateTowards(_followRoot.transform.rotation, targetRotation, 1f);
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

        private void CheckInputLockOn()
        {
            var inputModel = this.GetModel<IInputModel>();
            var cameraModel = this.GetModel<ICameraModel>();
            if (!inputModel.LockOn) return;
            if (cameraModel.IsLockOn.Value == false)
            {
                TryLockOn();
            }
            else
            {
                SetUnlock();
            }
        }

        private void CheckInputChangeTarget()
        {
            if (!this.GetModel<ICameraModel>().IsLockOn.Value) return;
            var changeTarget = this.GetModel<IInputModel>().ChangeTarget;
            if (Mathf.Abs(changeTarget) > ChangeTargetThreshold)
            {
                if (!_changeTargetTimerFlag)
                {
                    ChangeLockOnTarget(changeTarget < 0f);
                    _changeTargetTimer.Reset();
                    _changeTargetTimerFlag = true;
                }
                else
                {
                    if (_changeTargetTimer > ChangeTargetInterval)
                    {
                        _changeTargetTimerFlag = false;
                    }
                }
            }
            else
            {
                _changeTargetTimerFlag = false;
            }
        }
        
        private void SetUnlock()
        {
            lockDot.enabled = false;
            this.GetModel<ICameraModel>().LockTarget = null;
            this.GetModel<ICameraModel>().IsLockOn.Value = false;
        }

        private void TryLockOn()
        {
            var possibleHandles = this.SendQuery(_targetsQuery);
            possibleHandles = FilterTargetsInScreen(possibleHandles, _lockCenterRect);
            if (possibleHandles.Count == 0) return;
            this.GetModel<ICameraModel>().LockTarget = GetScreenCenterNearestTarget(possibleHandles);
            this.GetModel<ICameraModel>().IsLockOn.Value = true;
        }

        private void ChangeLockOnTarget(bool isLeft)
        {
            var possibleHandles = this.SendQuery(_targetsQuery);
            possibleHandles = FilterTargetsInScreen(possibleHandles, _screenRect);
            var currentTarget = this.GetModel<ICameraModel>().LockTarget;
            if (!possibleHandles.Contains(currentTarget))
            {
                SetUnlock();
                return;
            }

            possibleHandles.Sort((t1, t2) =>
                _camera.WorldToScreenPoint(t1.LockRoot.position).x
                    .CompareTo(_camera.WorldToScreenPoint(t2.LockRoot.position).x));

            var currentIndex = 0;
            for (var i = 0; i < possibleHandles.Count; i++)
            {
                if (currentTarget == possibleHandles[i])
                {
                    currentIndex = i;
                }
            }
            this.GetModel<ICameraModel>().LockTarget = isLeft switch
            {
                true when currentIndex > 0 => possibleHandles[currentIndex - 1],
                false when currentIndex < possibleHandles.Count - 1 => possibleHandles[currentIndex + 1],
                _ => this.GetModel<ICameraModel>().LockTarget
            };
        }


        // ReSharper disable Unity.PerformanceAnalysis
        private List<IActorHandle> FilterTargetsInScreen(List<IActorHandle> list, Rect screenRect)
        {
            List<IActorHandle> filterList = new();
            var visualCone = GeometryUtility.CalculateFrustumPlanes(_camera);
            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
            foreach (var target in list)
            {
                if (!GeometryUtility.TestPlanesAABB(visualCone, target.ActorTrans.GetComponent<Collider>().bounds))
                    continue;
                var screenPoint = _camera.WorldToScreenPoint(target.LockRoot.position);
                if (!screenRect.Contains(screenPoint))
                    continue;

                if (Physics.Linecast(_followRoot.transform.position, target.LockRoot.position, lockObstacleLayer))
                    continue;
                filterList.Add(target);
            }
            return filterList;
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private IActorHandle GetScreenCenterNearestTarget(List<IActorHandle> handles)
        {
            var result = handles[0];
            var point = _camera.WorldToScreenPoint(result.LockRoot.position);
            var sqrMinDistance = (new Vector2(point.x, point.y) - _screenCenter).sqrMagnitude;
            for (var i = 1; i < handles.Count; i++)
            {
                var tempPoint = _camera.WorldToScreenPoint(handles[i].LockRoot.position);
                var sqrDistance = (new Vector2(tempPoint.x, tempPoint.y) - _screenCenter).sqrMagnitude;
                if (sqrMinDistance <= sqrDistance) continue;
                sqrMinDistance = sqrDistance;
                result = handles[i];
            }
            return result;
        }
    }
}