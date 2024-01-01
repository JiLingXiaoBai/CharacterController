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
        private const float RotateSpeed = 5f;
        private const float CameraThreshold = 0.01f;
        private const float LockRange = 10f;

        // 俯仰角
        private float _cameraTargetPitch;
        // 偏航角
        private float _cameraTargetYaw;

        [SerializeField] private CinemachineVirtualCamera defaultCamera;
        [SerializeField] private Image lockDot;
        
        private GameObject _followRoot;
        private StateMachine<CameraState, string> _cameraStateMachine;
        private Collider[] _possibleTargets;
        private UnityEngine.Camera _camera;

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
            var inputModel = this.GetModel<IInputModel>();
            var cameraModel = this.GetModel<ICameraModel>();
            if (inputModel.LockOn)
            {
                if (cameraModel.IsLockOn.Value == false)
                {
                    TryLockOn();
                }
                else
                {
                    cameraModel.LockTarget = null;
                    cameraModel.IsLockOn.Value = false;
                }
            }
            // if (cameraModel.IsLockOn.Value)
            // {
            //     
            // }
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

        private void LockOnCameraUpdate()
        {
            var lockTarget = this.GetModel<ICameraModel>().LockTarget;
            if (lockTarget == null)
            {
                this.GetModel<ICameraModel>().IsLockOn.Value = false;
                lockDot.enabled = false;
                return;
            }
            lockDot.enabled = true;
            var lockRootPos = lockTarget.LockRoot.position;
            lockDot.rectTransform.position = _camera.WorldToScreenPoint(lockRootPos);
            var lockDir = lockRootPos - _followRoot.transform.position;
            var targetRotation = Quaternion.LookRotation(lockDir);
            var eulerAngles = targetRotation.eulerAngles;
            eulerAngles.x = ClampAngle(eulerAngles.x, BottomClamp, TopClamp);
            targetRotation = Quaternion.Euler(eulerAngles);
            _followRoot.transform.rotation =
                Quaternion.RotateTowards(_followRoot.transform.rotation, targetRotation, RotateSpeed);
        }

        private void TryLockOn()
        {
            var getPossibleLockOnTargetsQuery = new GetPossibleLockOnTargetsQuery(defaultCamera.transform.position,
                LockRange, LayerMask.GetMask("Enemy"), 20);
            var possibleHandles = this.SendQuery(getPossibleLockOnTargetsQuery);
            print(possibleHandles.Count);
            FilterTargetsInScreen(possibleHandles);
            SortTargetsByScreenPoints(possibleHandles);
            print(possibleHandles.Count);
            if (possibleHandles.Count == 0) return;
            this.GetModel<ICameraModel>().LockTarget = possibleHandles[0];
            this.GetModel<ICameraModel>().IsLockOn.Value = true;
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


        // ReSharper disable Unity.PerformanceAnalysis
        private void FilterTargetsInScreen(List<IActorHandle> list)
        {
            List<IActorHandle> filterList = new();
            var screenRect = _camera.rect;
            var visualCone = GeometryUtility.CalculateFrustumPlanes(_camera);
            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
            foreach (var target in list)
            {
                Vector2 screenPoint = _camera.WorldToScreenPoint(target.LockRoot.position);
                if (GeometryUtility.TestPlanesAABB(visualCone, target.ActorTrans.GetComponent<Collider>().bounds) && screenRect.Contains(screenPoint))
                {
                    filterList.Add(target);
                }
            }
            list = filterList;
        }

        private void SortTargetsByScreenPoints(List<IActorHandle> list)
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeCameraMain
            var nearestReferencePoint = _camera.rect.center;
            list.Sort((t1, t2) => Vector2.Distance(_camera.WorldToScreenPoint(t1.LockRoot.position), nearestReferencePoint)
                .CompareTo(Vector2.Distance(_camera.WorldToScreenPoint(t2.LockRoot.position), nearestReferencePoint)));
        }
    }
}