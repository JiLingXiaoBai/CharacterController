using UnityHFSM;

namespace ARPG.Actor.Player
{
    public class UnarmedEquipState : StateBase<PlayerStateConst.UnarmedBase>
    {
        public UnarmedEquipState(bool needsExitTime, bool isGhostState = false) : base(needsExitTime, isGhostState)
        {
        }
    }
}