using JLXB.Framework.FSM;

public class UnarmedCrouchEquipState : StateBase<PlayerStateConst.UnarmedCrouch>
{
    public UnarmedCrouchEquipState(bool needsExitTime, bool isGhostState = false) : base(needsExitTime, isGhostState)
    {
    }
}