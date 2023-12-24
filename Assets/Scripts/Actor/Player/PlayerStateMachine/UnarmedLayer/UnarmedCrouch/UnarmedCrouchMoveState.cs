using UnityHFSM;

namespace ARPG.Actor.Player
{
    public class UnarmedCrouchMoveState : StateBase<PlayerStateConst.UnarmedCrouch>
    {
        public UnarmedCrouchMoveState(bool needsExitTime, bool isGhostState = false) : base(needsExitTime, isGhostState)
        {
        }
    }
}