using UnityHFSM;
using ARPG.Animation;

namespace ARPG.Actor.Player
{
    public class UnarmedCrouchMoveState : StateBase<ActorStateConst.UnarmedCrouch>
    {
        public UnarmedCrouchMoveState(Actor actor) : base(needsExitTime: false, isGhostState: false)
        {
        }
    }
}