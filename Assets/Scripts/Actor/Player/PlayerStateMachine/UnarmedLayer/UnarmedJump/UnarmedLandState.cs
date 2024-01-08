using UnityHFSM;
using ARPG.Animation;

namespace ARPG.Actor.Player
{
    public class UnarmedLandState : StateBase<PlayerStateConst.UnarmedJump>
    {
        public UnarmedLandState(IAnimController animController) : base(needsExitTime: false, isGhostState: false)
        {
        }
    }
}