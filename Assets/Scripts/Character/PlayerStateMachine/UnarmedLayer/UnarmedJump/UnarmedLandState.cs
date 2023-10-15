using JLXB.Framework.FSM;

public class UnarmedLandState : StateBase<PlayerStateConst.UnarmedJump>
{
    public UnarmedLandState(bool needsExitTime, bool isGhostState = false) : base(needsExitTime, isGhostState)
    {
    }
}