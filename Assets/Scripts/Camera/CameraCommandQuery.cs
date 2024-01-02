using System.Collections.Generic;
using ARPG.Actor;
using UnityEngine;

namespace ARPG.Camera
{
    public class GetPossibleLockOnTargetsQuery : AbstractQuery<List<IActorHandle>>
    {
        private readonly Transform _viewer;
        private readonly float _radius;
        private readonly int _layerMask;
        private readonly Collider[] _results;

        public GetPossibleLockOnTargetsQuery(Transform viewer, float radius, int layerMask, int resultsMaxLength)
        {
            _viewer = viewer;
            _radius = radius;
            _layerMask = layerMask;
            _results = new Collider[resultsMaxLength];
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        protected override List<IActorHandle> OnDo()
        {
            var actorHandles = new List<IActorHandle>();
            var size = Physics.OverlapSphereNonAlloc(_viewer.position, _radius, _results, _layerMask);
            for (var i = 0; i < size; i++)
            {
                var actorHandle = _results[i].GetComponent<IActorHandle>();
                if(actorHandle == null) continue;
                var distance = Vector3.Distance(_viewer.position, actorHandle.LockRoot.position);
                if(distance > _radius) continue;
                actorHandles.Add(actorHandle);
            }
            return actorHandles;
        }
    }
}