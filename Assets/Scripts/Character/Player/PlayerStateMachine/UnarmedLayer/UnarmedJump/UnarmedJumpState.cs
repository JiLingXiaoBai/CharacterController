using UnityHFSM;

namespace ARPG.Character.Player
{
    public class UnarmedJumpState : StateBase<PlayerStateConst.UnarmedJump>
    {
        public UnarmedJumpState(bool needsExitTime, bool isGhostState = false) : base(needsExitTime, isGhostState)
        {
        }
    }
}