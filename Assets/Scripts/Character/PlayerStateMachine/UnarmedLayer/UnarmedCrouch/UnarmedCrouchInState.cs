using JLXB.Framework.FSM;

public class UnarmedCrouchInState : StateBase<PlayerStateConst.UnarmedCrouch>
{
    public UnarmedCrouchInState(bool needsExitTime, bool isGhostState = false) : base(needsExitTime, isGhostState)
    {
    }
}