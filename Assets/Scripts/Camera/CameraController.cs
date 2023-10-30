using Cinemachine;
using QFramework;
using UnityEngine;
using JLXB.Framework.FSM;

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
        [SerializeField] private Transform followCameraTarget;
        [SerializeField] private Transform lockOnCameraTarget;
        [SerializeField] private Transform closeUpCameraTarget;

        [SerializeField] private CinemachineVirtualCamera followCamera;

        [SerializeField] private CinemachineVirtualCamera lockOnCamera;

        [SerializeField] private CinemachineVirtualCamera closeUpCamera;

        public IArchitecture GetArchitecture()
        {
            return CharacterArchitecture.Interface;
        }

        private ICameraModel _model;

        private StateMachine<CameraState, string> _cameraStateMachine;

        private void Start()
        {
            _model = this.GetModel<ICameraModel>();
            _cameraStateMachine = new StateMachine<CameraState, string>();
            _cameraStateMachine.AddState(CameraState.Follow, onEnter: state => { Debug.Log("Follow"); });
            _cameraStateMachine.AddState(CameraState.LockOn, onEnter: state => { Debug.Log("LockOn"); });
            _cameraStateMachine.AddState(CameraState.CloseUp, onEnter: state => { Debug.Log("CloseUp"); });
            _cameraStateMachine.SetStartState(CameraState.Follow);
            _cameraStateMachine.AddTwoWayTriggerTransition("CameraStateChange", CameraState.Follow, CameraState.LockOn, t => _model.IsLockOn.Value);
            _cameraStateMachine.AddTriggerTransition("CameraStateChange",CameraState.Follow, CameraState.CloseUp, t => !_model.IsLockOn.Value && _model.IsClosingUp.Value);
            _cameraStateMachine.AddTriggerTransition("CameraStateChange",CameraState.LockOn, CameraState.CloseUp, t => _model.IsLockOn.Value && _model.IsClosingUp.Value);
            _cameraStateMachine.AddTriggerTransition("CameraStateChange",CameraState.CloseUp, CameraState.Follow, t => !_model.IsLockOn.Value && !_model.IsClosingUp.Value);
            _cameraStateMachine.AddTriggerTransition("CameraStateChange",CameraState.CloseUp, CameraState.LockOn, t => _model.IsLockOn.Value && !_model.IsClosingUp.Value);
            _cameraStateMachine.Init();
            
            _model.IsLockOn.RegisterWithInitValue(newCount =>
            {
                _cameraStateMachine.Trigger("CameraStateChange");

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            _model.IsClosingUp.RegisterWithInitValue(newCount =>
            {
                _cameraStateMachine.Trigger("CameraStateChange");

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            
        }

        private void Update()
        {
            _cameraStateMachine.OnLogic();
            
        }
    }
}