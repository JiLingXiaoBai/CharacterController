using UnityHFSM;
using ARPG.Animation;

namespace ARPG.Actor.Player
{
    public class UnarmedCrouchInState : StateBase<ActorStateConst.UnarmedCrouch>
    {
        public UnarmedCrouchInState(Actor actor) : base(needsExitTime: false, isGhostState: false)
        {
        }
    }
}