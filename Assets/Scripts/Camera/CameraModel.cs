
using QFramework;
using JLXB.Framework.FSM;
namespace ARPG.Camera
{
    public interface ICameraModel : IModel
    {
        
    }
    
    public class CameraModel : AbstractModel, ICameraModel
    {
        protected override void OnInit()
        {
            
        }
    }
}
