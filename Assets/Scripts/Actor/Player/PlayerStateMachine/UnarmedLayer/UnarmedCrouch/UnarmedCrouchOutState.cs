using UnityHFSM;

namespace ARPG.Actor.Player
{
    public class UnarmedCrouchOutState : StateBase<PlayerStateConst.UnarmedCrouch>
    {
        public UnarmedCrouchOutState(bool needsExitTime, bool isGhostState = false) : base(needsExitTime, isGhostState)
        {
        }
    }
}