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

        private StateMachine<CameraState, string> _cameraStateMachine;
        private bool _isLockOn;
        private bool _isClosingUp;

        private void Start()
        {
            _cameraStateMachine = new StateMachine<CameraState, string>();
            _cameraStateMachine.AddState(CameraState.Follow, onEnter: state => { Debug.Log("Follow"); });
            _cameraStateMachine.AddState(CameraState.LockOn, onEnter: state => { Debug.Log("LockOn"); });
            _cameraStateMachine.AddState(CameraState.CloseUp, onEnter: state => { Debug.Log("CloseUp"); });
            _cameraStateMachine.SetStartState(CameraState.Follow);
            _cameraStateMachine.AddTwoWayTransition(CameraState.Follow, CameraState.LockOn, t => _isLockOn);
            _cameraStateMachine.AddTransition(CameraState.Follow, CameraState.CloseUp, t => !_isLockOn && _isClosingUp);
            _cameraStateMachine.AddTransition(CameraState.LockOn, CameraState.CloseUp, t => _isLockOn && _isClosingUp);
            _cameraStateMachine.AddTransition(CameraState.CloseUp, CameraState.Follow, t => !_isLockOn && !_isClosingUp);
            _cameraStateMachine.AddTransition(CameraState.CloseUp, CameraState.LockOn, t => _isLockOn && !_isClosingUp);
            _cameraStateMachine.Init();
        }

        private void Update()
        {
            _cameraStateMachine.OnLogic();
        }
    }
}