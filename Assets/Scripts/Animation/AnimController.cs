using Animancer;
using UnityEngine;
using UnityHFSM;

namespace ARPG.Animation
{
    public interface IAnimController : IController
    {
        AnimancerComponent Animancer { get; set; }
    }
    
    [RequireComponent(typeof(AnimancerComponent))]
    [RequireComponent(typeof(Animator))]
    public class AnimController : AbstractController, IAnimController
    {
        public AnimancerComponent Animancer { get; set; }

        private void Awake()
        {
            Animancer = gameObject.GetComponent<AnimancerComponent>();
            Animancer.Animator = gameObject.GetComponent<Animator>();
        }
    }
}