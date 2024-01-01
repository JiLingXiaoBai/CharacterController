using ARPG.Camera;
using ARPG.Input;
using ARPG.Movement;
using UnityEngine;

namespace ARPG.Actor.Player
{
    public class PlayerHandle : MonoBehaviour, IActorHandle
    {
        [SerializeField] private GameObject cameraHandle;
        [SerializeField] private GameObject modelHandle;
        [SerializeField] private Transform lockRoot;
        public Actor Actor { get; private set; }
        public Transform LockRoot => lockRoot;
        
        private void Awake()
        {
            Actor = new Actor();
            Actor.RegisterModel<IInputModel>(new PlayerInputModel());
            Actor.RegisterModel<ICameraModel>(new CameraModel());
            
            Actor.BindController<CameraController>(cameraHandle);
            Actor.BindController<MovementController>(modelHandle);
            Actor.Init();
        }
    }
}