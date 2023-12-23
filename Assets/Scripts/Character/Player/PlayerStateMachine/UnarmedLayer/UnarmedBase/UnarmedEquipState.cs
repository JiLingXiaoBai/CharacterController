using UnityHFSM;

namespace ARPG.Character.Player
{
    public class UnarmedEquipState : StateBase<PlayerStateConst.UnarmedBase>
    {
        public UnarmedEquipState(bool needsExitTime, bool isGhostState = false) : base(needsExitTime, isGhostState)
        {
        }
    }
}