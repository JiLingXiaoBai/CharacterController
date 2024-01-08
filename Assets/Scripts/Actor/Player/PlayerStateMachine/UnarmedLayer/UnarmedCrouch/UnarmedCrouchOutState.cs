using UnityHFSM;
using ARPG.Animation;

namespace ARPG.Actor.Player
{
    public class UnarmedCrouchOutState : StateBase<PlayerStateConst.UnarmedCrouch>
    {
        public UnarmedCrouchOutState(IAnimController animController) : base(needsExitTime: false, isGhostState: false)
        {
        }
    }
}