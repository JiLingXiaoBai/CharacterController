using JLXB.Framework.FSM;

public class UnarmedFallState : StateBase<PlayerStateConst.UnarmedJump>
{
    public UnarmedFallState(bool needsExitTime, bool isGhostState = false) : base(needsExitTime, isGhostState)
    {
    }
}