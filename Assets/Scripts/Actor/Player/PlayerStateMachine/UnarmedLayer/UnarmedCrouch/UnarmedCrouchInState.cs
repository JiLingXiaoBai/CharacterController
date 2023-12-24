using UnityHFSM;

namespace ARPG.Actor.Player
{
    public class UnarmedCrouchInState : StateBase<PlayerStateConst.UnarmedCrouch>
    {
        public UnarmedCrouchInState(bool needsExitTime, bool isGhostState = false) : base(needsExitTime, isGhostState)
        {
        }
    }
}