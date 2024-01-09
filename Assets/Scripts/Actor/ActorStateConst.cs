namespace ARPG.Actor
{
    public static class ActorStateConst
    {
        public enum ActorSuperState
        {
            Unarmed,
            Armed,
        }

        public enum Unarmed
        {
            CrouchMotion,
            JumpMotion,
            BaseMotion
        }

        public enum UnarmedCrouch
        {
            CrouchIn,
            CrouchMove,
            CrouchOut,
            CrouchEquip
        }

        public enum UnarmedJump
        {
            Jump,
            Fall,
            Land
        }

        public enum UnarmedBase
        {
            Move,
            Equip,
        }
    }
}