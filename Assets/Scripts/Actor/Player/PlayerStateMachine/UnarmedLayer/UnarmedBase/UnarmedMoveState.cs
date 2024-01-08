using UnityHFSM;
using ARPG.Animation;

namespace ARPG.Actor.Player
{
    public class UnarmedMoveState : StateBase<PlayerStateConst.UnarmedBase>
    {
        public UnarmedMoveState(IAnimController animController) : base(needsExitTime: false, isGhostState: false)
        {
        }
    }
}