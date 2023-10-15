using JLXB.Framework.FSM;

public class UnarmedCrouchMoveState : StateBase<PlayerStateConst.UnarmedCrouch>
{
    public UnarmedCrouchMoveState(bool needsExitTime, bool isGhostState = false) : base(needsExitTime, isGhostState)
    {
    }
}