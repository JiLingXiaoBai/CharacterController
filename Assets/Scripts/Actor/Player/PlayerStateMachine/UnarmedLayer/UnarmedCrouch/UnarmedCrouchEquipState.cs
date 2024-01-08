using UnityHFSM;
using ARPG.Animation;

namespace ARPG.Actor.Player
{
    public class UnarmedCrouchEquipState : StateBase<PlayerStateConst.UnarmedCrouch>
    {
        public UnarmedCrouchEquipState(IAnimController animController) : base(needsExitTime: false, isGhostState: false)
        {
        }
    }
}