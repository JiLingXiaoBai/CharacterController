using UnityHFSM;

namespace ARPG.Character.Player
{
    public class UnarmedCrouchOutState : StateBase<PlayerStateConst.UnarmedCrouch>
    {
        public UnarmedCrouchOutState(bool needsExitTime, bool isGhostState = false) : base(needsExitTime, isGhostState)
        {
        }
    }
}