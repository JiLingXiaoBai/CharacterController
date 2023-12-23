using UnityHFSM;

namespace ARPG.Character.Player
{
    public class UnarmedCrouchEquipState : StateBase<PlayerStateConst.UnarmedCrouch>
    {
        public UnarmedCrouchEquipState(bool needsExitTime, bool isGhostState = false) : base(needsExitTime,
            isGhostState)
        {
        }
    }
}