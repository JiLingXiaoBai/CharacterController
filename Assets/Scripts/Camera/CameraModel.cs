using ARPG.Actor;
using UnityEngine;

namespace ARPG.Camera
{
    public interface ICameraModel : IModel
    {
        BindableProperty<bool> IsLockOn{ get; }
        BindableProperty<bool> IsClosingUp{ get; }
        IActorHandle LockTarget { get; set; }
        Transform CameraTrans { get; set; }
    }
    
    public class CameraModel : AbstractModel, ICameraModel
    {
        public BindableProperty<bool> IsLockOn { get; } = new BindableProperty<bool>();
        public BindableProperty<bool> IsClosingUp { get; } = new BindableProperty<bool>();
        public IActorHandle LockTarget { get; set; }
        public Transform CameraTrans { get; set; }

        protected override void OnInit()
        {
            IsLockOn.SetValueWithoutEvent(false);
            IsClosingUp.SetValueWithoutEvent(false);
        }
    }
}
