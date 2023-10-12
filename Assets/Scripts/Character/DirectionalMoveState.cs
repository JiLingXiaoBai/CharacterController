using JLXB.Framework.FSM;

public class DirectionalMoveState : StateBase<PlayerStateMachine.GroundLocomotionState>
{
    public DirectionalMoveState(bool needsExitTime, bool isGhostState = false) : base(needsExitTime, isGhostState)
    {
    }
}