using UnityHFSM;
using ARPG.Animation;

namespace ARPG.Actor.Player
{
    public class UnarmedFallState : StateBase<ActorStateConst.UnarmedJump>
    {
        public UnarmedFallState(Actor actor) : base(needsExitTime: false, isGhostState: false)
        {
        }
    }
}