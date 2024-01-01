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
        public Actor ActorObject { get; private set; }
        public Transform LockRoot => lockRoot;
        public Transform ActorTrans { get; private set; }

        private void Awake()
        {
            ActorTrans = transform;
            ActorObject = new Actor();
            ActorObject.RegisterModel<IInputModel>(new PlayerInputModel());
            ActorObject.RegisterModel<ICameraModel>(new CameraModel());
            
            ActorObject.BindController<CameraController>(cameraHandle);
            ActorObject.BindController<MovementController>(modelHandle);
            ActorObject.Init();
        }
    }
}