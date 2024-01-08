using UnityHFSM;
using ARPG.Animation;

namespace ARPG.Actor.Player
{
    public class UnarmedFallState : StateBase<PlayerStateConst.UnarmedJump>
    {
        public UnarmedFallState(IAnimController animController) : base(needsExitTime: false, isGhostState: false)
        {
        }
    }
}