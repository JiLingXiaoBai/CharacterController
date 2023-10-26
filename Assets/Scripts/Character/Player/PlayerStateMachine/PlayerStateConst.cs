namespace ARPG.Character.Player
{
    public static class PlayerStateConst
    {
        public enum PlayerSuperState
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