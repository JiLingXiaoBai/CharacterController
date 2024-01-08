using UnityHFSM;
using ARPG.Animation;

namespace ARPG.Actor.Player
{
    public class UnarmedJumpState : StateBase<PlayerStateConst.UnarmedJump>
    {
        public UnarmedJumpState(IAnimController animController) : base(needsExitTime: false, isGhostState: false)
        {
        }
    }
}