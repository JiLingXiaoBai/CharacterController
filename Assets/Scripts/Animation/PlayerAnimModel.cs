using Animancer;
using ARPG.Actor;
using UnityHFSM;

namespace ARPG.Animation
{
    public class PlayerAnimModel: AbstractModel, IAnimModel
    {
        protected override void OnInit()
        {
            
        }

        public AnimancerComponent Animancer { get; set; }
        public StateMachine<ActorStateConst.ActorSuperState> ActorStateMachine { get; set; }
    }
}