using Animancer;
using UnityEngine;
using UnityHFSM;

namespace ARPG.Animation
{
    [RequireComponent(typeof(AnimancerComponent))]
    [RequireComponent(typeof(Animator))]
    public class AnimController : AbstractController
    {
        private AnimancerComponent _animancer;

        private void Awake()
        {
            _animancer = gameObject.GetComponent<AnimancerComponent>();
            _animancer.Animator = gameObject.GetComponent<Animator>();
        }
    }
}