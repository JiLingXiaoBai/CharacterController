using JLXB.Framework.FSM;

namespace ARPG.Character.Player
{
    public class UnarmedMoveState : StateBase<PlayerStateConst.UnarmedBase>
    {
        public UnarmedMoveState(bool needsExitTime, bool isGhostState = false) : base(needsExitTime, isGhostState)
        {
        }
    }
}