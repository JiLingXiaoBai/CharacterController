using JLXB.Framework.FSM;

namespace ARPG.Character.Player
{
    public class UnarmedLandState : StateBase<PlayerStateConst.UnarmedJump>
    {
        public UnarmedLandState(bool needsExitTime, bool isGhostState = false) : base(needsExitTime, isGhostState)
        {
        }
    }
}