using UnityHFSM;

namespace ARPG.Actor.Player
{
    public class PlayerStateMachine
    {
        private readonly HybridStateMachine<ActorStateConst.ActorSuperState> _stateMachine;

        private readonly HybridStateMachine<ActorStateConst.ActorSuperState, ActorStateConst.Unarmed, string>
            _unarmed;

        private readonly StateMachine<ActorStateConst.Unarmed, ActorStateConst.UnarmedBase, string> _unarmedBase;
        private readonly StateMachine<ActorStateConst.Unarmed, ActorStateConst.UnarmedCrouch, string> _unarmedCrouch;
        private readonly StateMachine<ActorStateConst.Unarmed, ActorStateConst.UnarmedJump, string> _unarmedJump;


        public PlayerStateMachine(Actor actor)
        {
            _stateMachine = new HybridStateMachine<ActorStateConst.ActorSuperState>();
            _unarmed = new HybridStateMachine<ActorStateConst.ActorSuperState, ActorStateConst.Unarmed, string>();

            _unarmedBase = new StateMachine<ActorStateConst.Unarmed, ActorStateConst.UnarmedBase, string>();
            var unarmedMoveState = new UnarmedMoveState(actor);
            _unarmedBase.AddState(ActorStateConst.UnarmedBase.Move, unarmedMoveState);
            var unarmedEquipState = new UnarmedEquipState(actor);
            _unarmedBase.AddState(ActorStateConst.UnarmedBase.Equip, unarmedEquipState);
            _unarmed.AddState(ActorStateConst.Unarmed.BaseMotion, _unarmedBase);

            _unarmedCrouch = new StateMachine<ActorStateConst.Unarmed, ActorStateConst.UnarmedCrouch, string>();
            var unarmedCrouchEquipState = new UnarmedCrouchEquipState(actor);
            _unarmedCrouch.AddState(ActorStateConst.UnarmedCrouch.CrouchEquip, unarmedCrouchEquipState);
            var unarmedCrouchInState = new UnarmedCrouchInState(actor);
            _unarmedCrouch.AddState(ActorStateConst.UnarmedCrouch.CrouchIn, unarmedCrouchInState);
            var unarmedCrouchOutState = new UnarmedCrouchOutState(actor);
            _unarmedCrouch.AddState(ActorStateConst.UnarmedCrouch.CrouchOut, unarmedCrouchOutState);
            var unarmedCrouchMoveState = new UnarmedCrouchMoveState(actor);
            _unarmedCrouch.AddState(ActorStateConst.UnarmedCrouch.CrouchMove, unarmedCrouchMoveState);
            _unarmed.AddState(ActorStateConst.Unarmed.CrouchMotion, _unarmedCrouch);

            _unarmedJump = new StateMachine<ActorStateConst.Unarmed, ActorStateConst.UnarmedJump, string>();
            var unarmedFallState = new UnarmedFallState(actor);
            _unarmedJump.AddState(ActorStateConst.UnarmedJump.Fall, unarmedFallState);
            var unarmedJumpState = new UnarmedJumpState(actor);
            _unarmedJump.AddState(ActorStateConst.UnarmedJump.Jump, unarmedJumpState);
            var unarmedLandState = new UnarmedLandState(actor);
            _unarmedJump.AddState(ActorStateConst.UnarmedJump.Land, unarmedLandState);
            _unarmed.AddState(ActorStateConst.Unarmed.JumpMotion, _unarmedJump);

            _stateMachine.AddState(ActorStateConst.ActorSuperState.Unarmed, _unarmed);
            _stateMachine.Init();
        }

        public void OnLogic()
        {
            _stateMachine.OnLogic();
        }
    }
}