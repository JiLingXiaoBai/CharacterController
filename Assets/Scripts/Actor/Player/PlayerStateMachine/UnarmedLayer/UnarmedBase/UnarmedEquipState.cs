using UnityHFSM;
using ARPG.Animation;

namespace ARPG.Actor.Player
{
    public class UnarmedEquipState : StateBase<ActorStateConst.UnarmedBase>
    {
        public UnarmedEquipState(Actor actor) : base(needsExitTime: false, isGhostState: false)
        {
        }
    }
}