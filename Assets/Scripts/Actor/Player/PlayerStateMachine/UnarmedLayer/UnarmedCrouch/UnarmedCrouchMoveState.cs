using UnityHFSM;
using ARPG.Animation;

namespace ARPG.Actor.Player
{
    public class UnarmedCrouchMoveState : StateBase<PlayerStateConst.UnarmedCrouch>
    {
        public UnarmedCrouchMoveState(IAnimController animController) : base(needsExitTime: false, isGhostState: false)
        {
        }
    }
}