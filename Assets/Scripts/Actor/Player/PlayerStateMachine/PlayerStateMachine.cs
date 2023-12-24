using UnityHFSM;

namespace ARPG.Actor.Player
{
    public class PlayerStateMachine
    {
        private readonly HybridStateMachine<PlayerStateConst.PlayerSuperState> _stateMachine;

        private readonly HybridStateMachine<PlayerStateConst.PlayerSuperState, PlayerStateConst.Unarmed, string>
            _unarmed;

        private readonly StateMachine<PlayerStateConst.Unarmed, PlayerStateConst.UnarmedBase, string> _unarmedBase;
        private readonly StateMachine<PlayerStateConst.Unarmed, PlayerStateConst.UnarmedCrouch, string> _unarmedCrouch;
        private readonly StateMachine<PlayerStateConst.Unarmed, PlayerStateConst.UnarmedJump, string> _unarmedJump;


        public PlayerStateMachine()
        {
            _stateMachine = new HybridStateMachine<PlayerStateConst.PlayerSuperState>();
            _unarmed = new HybridStateMachine<PlayerStateConst.PlayerSuperState, PlayerStateConst.Unarmed, string>();

            _unarmedBase = new StateMachine<PlayerStateConst.Unarmed, PlayerStateConst.UnarmedBase, string>();
            var unarmedMoveState = new UnarmedMoveState(false);
            _unarmedBase.AddState(PlayerStateConst.UnarmedBase.Move, unarmedMoveState);
            var unarmedEquipState = new UnarmedEquipState(false);
            _unarmedBase.AddState(PlayerStateConst.UnarmedBase.Equip, unarmedEquipState);
            _unarmed.AddState(PlayerStateConst.Unarmed.BaseMotion, _unarmedBase);

            _unarmedCrouch = new StateMachine<PlayerStateConst.Unarmed, PlayerStateConst.UnarmedCrouch, string>();
            var unarmedCrouchEquipState = new UnarmedCrouchEquipState(false);
            _unarmedCrouch.AddState(PlayerStateConst.UnarmedCrouch.CrouchEquip, unarmedCrouchEquipState);
            var unarmedCrouchInState = new UnarmedCrouchInState(false);
            _unarmedCrouch.AddState(PlayerStateConst.UnarmedCrouch.CrouchIn, unarmedCrouchInState);
            var unarmedCrouchOutState = new UnarmedCrouchOutState(false);
            _unarmedCrouch.AddState(PlayerStateConst.UnarmedCrouch.CrouchOut, unarmedCrouchOutState);
            var unarmedCrouchMoveState = new UnarmedCrouchMoveState(false);
            _unarmedCrouch.AddState(PlayerStateConst.UnarmedCrouch.CrouchMove, unarmedCrouchMoveState);
            _unarmed.AddState(PlayerStateConst.Unarmed.CrouchMotion, _unarmedCrouch);

            _unarmedJump = new StateMachine<PlayerStateConst.Unarmed, PlayerStateConst.UnarmedJump, string>();
            var unarmedFallState = new UnarmedFallState(false);
            _unarmedJump.AddState(PlayerStateConst.UnarmedJump.Fall, unarmedFallState);
            var unarmedJumpState = new UnarmedJumpState(false);
            _unarmedJump.AddState(PlayerStateConst.UnarmedJump.Jump, unarmedJumpState);
            var unarmedLandState = new UnarmedLandState(false);
            _unarmedJump.AddState(PlayerStateConst.UnarmedJump.Land, unarmedLandState);
            _unarmed.AddState(PlayerStateConst.Unarmed.JumpMotion, _unarmedJump);

            _stateMachine.AddState(PlayerStateConst.PlayerSuperState.Unarmed, _unarmed);
            _stateMachine.Init();
        }

        public void OnLogic()
        {
            _stateMachine.OnLogic();
        }
    }
}