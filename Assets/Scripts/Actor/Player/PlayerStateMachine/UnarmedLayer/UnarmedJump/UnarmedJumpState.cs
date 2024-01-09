using UnityHFSM;
using ARPG.Animation;

namespace ARPG.Actor.Player
{
    public class UnarmedJumpState : StateBase<ActorStateConst.UnarmedJump>
    {
        public UnarmedJumpState(Actor actor) : base(needsExitTime: false, isGhostState: false)
        {
        }
    }
}