using UnityHFSM;
using ARPG.Animation;

namespace ARPG.Actor.Player
{
    public class UnarmedLandState : StateBase<ActorStateConst.UnarmedJump>
    {
        public UnarmedLandState(Actor actor) : base(needsExitTime: false, isGhostState: false)
        {
        }
    }
}