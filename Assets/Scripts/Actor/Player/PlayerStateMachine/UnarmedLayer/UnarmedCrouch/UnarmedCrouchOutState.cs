using UnityHFSM;
using ARPG.Animation;

namespace ARPG.Actor.Player
{
    public class UnarmedCrouchOutState : StateBase<ActorStateConst.UnarmedCrouch>
    {
        public UnarmedCrouchOutState(Actor actor) : base(needsExitTime: false, isGhostState: false)
        {
        }
    }
}