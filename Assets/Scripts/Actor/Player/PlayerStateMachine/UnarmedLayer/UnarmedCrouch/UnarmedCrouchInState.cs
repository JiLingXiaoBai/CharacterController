using UnityHFSM;
using ARPG.Animation;

namespace ARPG.Actor.Player
{
    public class UnarmedCrouchInState : StateBase<PlayerStateConst.UnarmedCrouch>
    {
        public UnarmedCrouchInState(IAnimController animController) : base(needsExitTime: false, isGhostState: false)
        {
        }
    }
}