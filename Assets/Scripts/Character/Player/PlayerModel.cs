using QFramework;

namespace ARPG.Character.Player
{
    public interface IPlayerModel : IModel
    {
    }

    public class PlayerModel : AbstractModel, IPlayerModel
    {
        protected override void OnInit()
        {
        }
    }
}