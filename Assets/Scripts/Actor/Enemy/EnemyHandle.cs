using ARPG.Actor;
using UnityEngine;

public class EnemyHandle : MonoBehaviour, IActorHandle
{
    [SerializeField] private Transform lockRoot;
    public Actor ActorObject { get; private set; }
    public Transform LockRoot => lockRoot;
    public Transform ActorTrans { get; private set; }

    private void Awake()
    {
        ActorObject = new Actor();
        ActorTrans = transform;
    }
}
