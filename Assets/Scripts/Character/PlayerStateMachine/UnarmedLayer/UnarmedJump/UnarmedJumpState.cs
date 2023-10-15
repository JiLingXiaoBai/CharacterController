using JLXB.Framework.FSM;

public class UnarmedJumpState : StateBase<PlayerStateConst.UnarmedJump>
{
    public UnarmedJumpState(bool needsExitTime, bool isGhostState = false) : base(needsExitTime, isGhostState)
    {
    }
}