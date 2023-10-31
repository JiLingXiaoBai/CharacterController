using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ARPG.Camera
{
    public class CameraObstaclesDither : MonoBehaviour
    {
        public LayerMask obstacleLayer;
        public Material ditherMaterial;
        public Transform cameraTarget;
        private RaycastHit[] _obstacleHits;
        private RaycastHit _playerHit;
        private Dictionary<MeshRenderer, DitherObj> _ditherObjs;
        private List<MeshRenderer> _removeDitherKeys;
        
        
        private class DitherObj
        {
            public readonly Material[] Materials;
            public bool Flag;

            public DitherObj(Renderer renderer, bool flag)
            {
                Materials = new Material[renderer.materials.Length];
                for (var i = 0; i < renderer.materials.Length; i++)
                {
                    Materials[i] = renderer.materials[i];
                }

                Flag = flag;
            }
        }

        private void Awake()
        {
            _ditherObjs = new Dictionary<MeshRenderer, DitherObj>();
            _removeDitherKeys = new List<MeshRenderer>();
            _obstacleHits = new RaycastHit[32];
        }
        
        private void FixedUpdate()
        {
            UpdateDitherObstacles();
        }
        
        private void UpdateDitherObstacles()
        {
            var cameraPos = transform.position;
            var targetPos = cameraTarget.position;
            var viewDir = (targetPos - cameraPos).normalized;

            foreach (var ditherObj in _ditherObjs)
            {
                ditherObj.Value.Flag = false;
            }

            if (!Physics.Raycast(cameraPos, viewDir, out _playerHit)) return;
            if (!_playerHit.transform.CompareTag("Player"))
            {
                var distance = Vector3.Distance(cameraPos, targetPos);
                var hitCount = Physics.RaycastNonAlloc(cameraPos, viewDir, _obstacleHits, distance, obstacleLayer);
                for (var i = 0; i < hitCount; i++)
                {
                    if (_obstacleHits[i].transform.CompareTag("Player"))
                    {
                        continue;
                    }

                    var renderers = _obstacleHits[i].transform.GetComponentsInChildren<MeshRenderer>();
                    foreach (var meshRenderer in renderers)
                    {
                        if (!_ditherObjs.ContainsKey(meshRenderer))
                        {
                            var ditherObj = new DitherObj(meshRenderer, true);
                            _ditherObjs.Add(meshRenderer, ditherObj);

                            var tempMaterials = new Material[meshRenderer.materials.Length];
                            for (var j = 0; j < meshRenderer.materials.Length; j++)
                            {
                                tempMaterials[j] = ditherMaterial;
                            }

                            meshRenderer.materials = tempMaterials;
                        }
                        else
                        {
                            _ditherObjs[meshRenderer].Flag = true;
                        }
                    }
                }
            }

            foreach (var ditherObj in _ditherObjs.Where(ditherObj => ditherObj.Value.Flag == false))
            {
                ditherObj.Key.materials = ditherObj.Value.Materials;
                _removeDitherKeys.Add(ditherObj.Key);
            }

            foreach (var removeDitherObj in _removeDitherKeys)
            {
                _ditherObjs.Remove(removeDitherObj);
            }

            _removeDitherKeys.Clear();
        }

    }
}