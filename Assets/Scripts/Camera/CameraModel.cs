
using QFramework;
namespace ARPG.Camera
{
    public interface ICameraModel : IModel
    {
        BindableProperty<bool> IsLockOn{ get; }
        BindableProperty<bool> IsClosingUp{ get; }
    }
    
    public class CameraModel : AbstractModel, ICameraModel
    {
        public BindableProperty<bool> IsLockOn { get; } = new BindableProperty<bool>();
        public BindableProperty<bool> IsClosingUp { get; } = new BindableProperty<bool>();
        protected override void OnInit()
        {
            IsLockOn.SetValueWithoutEvent(false);
            IsClosingUp.SetValueWithoutEvent(false);
        }
    }
}
