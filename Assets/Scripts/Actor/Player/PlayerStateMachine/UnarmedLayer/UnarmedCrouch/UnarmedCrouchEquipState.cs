using UnityHFSM;
using ARPG.Animation;

namespace ARPG.Actor.Player
{
    public class UnarmedCrouchEquipState : StateBase<ActorStateConst.UnarmedCrouch>
    {
        public UnarmedCrouchEquipState(Actor actor) : base(needsExitTime: false, isGhostState: false)
        {
        }
    }
}