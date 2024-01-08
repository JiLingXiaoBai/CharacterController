using UnityHFSM;
using ARPG.Animation;

namespace ARPG.Actor.Player
{
    public class UnarmedEquipState : StateBase<PlayerStateConst.UnarmedBase>
    {
        public UnarmedEquipState(IAnimController animController) : base(needsExitTime: false, isGhostState: false)
        {
        }
    }
}