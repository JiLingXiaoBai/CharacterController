using UnityHFSM;

namespace ARPG.Character.Player
{
    public class UnarmedFallState : StateBase<PlayerStateConst.UnarmedJump>
    {
        public UnarmedFallState(bool needsExitTime, bool isGhostState = false) : base(needsExitTime, isGhostState)
        {
        }
    }
}