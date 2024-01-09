using UnityHFSM;
using ARPG.Animation;

namespace ARPG.Actor.Player
{
    public class UnarmedMoveState : StateBase<ActorStateConst.UnarmedBase>
    {
        public UnarmedMoveState(Actor actor) : base(needsExitTime: false, isGhostState: false)
        {
        }
    }
}