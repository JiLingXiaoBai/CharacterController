using System.Collections.Generic;
using ARPG.Actor;
using UnityEngine;

namespace ARPG.Camera
{
    public class GetPossibleLockOnTargetsQuery : AbstractQuery<List<IActorHandle>>
    {
        private readonly Vector3 _viewer;
        private readonly float _radius;
        private readonly int _layerMask;
        private readonly int _resultsMaxLength;

        public GetPossibleLockOnTargetsQuery(Vector3 viewer, float radius, int layerMask, int resultsMaxLength)
        {
            _viewer = viewer;
            _radius = radius;
            _layerMask = layerMask;
            _resultsMaxLength = resultsMaxLength;
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        protected override List<IActorHandle> OnDo()
        {
            var actorHandles = new List<IActorHandle>();
            var results = new Collider[_resultsMaxLength];
            var size = Physics.OverlapSphereNonAlloc(_viewer, _radius, results, _layerMask);
            for (var i = 0; i < size; i++)
            {
                var actorHandle = results[i].GetComponent<IActorHandle>();
                if(actorHandle == null) continue;
                actorHandles.Add(actorHandle);
                Debug.DrawLine(_viewer, actorHandle.LockRoot.position, Color.green, _radius);
            }
            return actorHandles;
        }
    }
}