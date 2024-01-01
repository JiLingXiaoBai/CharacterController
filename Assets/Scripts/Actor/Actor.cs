using UnityEngine;

namespace ARPG.Actor
{
    public class Actor : AbstractActor<Actor>
    {
        protected override void OnInit()
        {
            base.OnInit();
        }
    }

    public interface IActorHandle
    {
        public Actor ActorObject { get; }
        public Transform LockRoot { get; }
        public Transform ActorTrans { get; }
    }
}