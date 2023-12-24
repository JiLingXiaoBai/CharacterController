using UnityHFSM;

namespace ARPG.Actor.Player
{
    public class UnarmedLandState : StateBase<PlayerStateConst.UnarmedJump>
    {
        public UnarmedLandState(bool needsExitTime, bool isGhostState = false) : base(needsExitTime, isGhostState)
        {
        }
    }
}