using JLXB.Framework.FSM;

public class PlayerStateMachine
{
    public enum PlayerSuperState
    {
        OnGround,
        InAir,
    }

    public enum GroundState
    {
        Locomotion,
        Combat,
    }

    public enum AirState
    {
        Locomotion,
        Combat,
    }

    public enum GroundLocomotionState
    {
        DirectionalMove,
        StrafeMove,
        Dodge,
    }

    public enum GroundCombatState
    {
        
    }
    
    public enum AirLocomotionState
    {
        Jump,
        Land,
        Falling,
    }

    public enum AirCombatState
    {
        
    }
    
    private HybridStateMachine<PlayerSuperState, string> _stateMachine;
    private HybridStateMachine<PlayerSuperState, GroundState, string> _groundStateMachine;
    private HybridStateMachine<PlayerSuperState, AirState, string> _airStateMachine;
    private HybridStateMachine<GroundState, GroundLocomotionState, string> _groundLocomotionStateMachine;
    private HybridStateMachine<GroundState, GroundCombatState, string> _groundCombatStateMachine;
    private HybridStateMachine<AirState, AirLocomotionState, string> _airLocomotionStateMachine;
    private HybridStateMachine<AirState, AirCombatState, string> _airCombatStateMachine;

    public PlayerStateMachine()
    {
        _stateMachine = new HybridStateMachine<PlayerSuperState, string>();
        _groundStateMachine = new HybridStateMachine<PlayerSuperState, GroundState, string>();
        _airStateMachine = new HybridStateMachine<PlayerSuperState, AirState, string>();
        _groundLocomotionStateMachine = new HybridStateMachine<GroundState, GroundLocomotionState, string>();
        _groundCombatStateMachine = new HybridStateMachine<GroundState, GroundCombatState, string>();
        _airLocomotionStateMachine = new HybridStateMachine<AirState, AirLocomotionState, string>();
        _airCombatStateMachine = new HybridStateMachine<AirState, AirCombatState, string>();
        _stateMachine.AddState(PlayerSuperState.OnGround, _groundStateMachine);
        _stateMachine.AddState(PlayerSuperState.InAir, _airStateMachine);
        _groundStateMachine.AddState(GroundState.Locomotion, _groundLocomotionStateMachine);
        _groundStateMachine.AddState(GroundState.Combat, _groundCombatStateMachine);
        _airStateMachine.AddState(AirState.Locomotion, _airLocomotionStateMachine);
        _airStateMachine.AddState(AirState.Combat, _airCombatStateMachine);
        _stateMachine.Init();
    }

    public void OnLogic()
    {
        _stateMachine.OnLogic();
    }
        
}