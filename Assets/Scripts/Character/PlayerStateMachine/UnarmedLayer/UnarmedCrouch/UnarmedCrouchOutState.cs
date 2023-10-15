using JLXB.Framework.FSM;

public class UnarmedCrouchOutState : StateBase<PlayerStateConst.UnarmedCrouch>
{
    public UnarmedCrouchOutState(bool needsExitTime, bool isGhostState = false) : base(needsExitTime, isGhostState)
    {
    }
}