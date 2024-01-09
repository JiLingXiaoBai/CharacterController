using Animancer;
using UnityHFSM;
using ARPG.Actor;

namespace ARPG.Animation
{
    public interface IAnimModel
    {
        AnimancerComponent Animancer { get; set; }
        StateMachine<ActorStateConst.ActorSuperState> ActorStateMachine { get; set; }
    }
}