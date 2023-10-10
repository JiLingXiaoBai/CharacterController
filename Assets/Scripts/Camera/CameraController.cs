using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTarget;
    public LayerMask obstacleLayer;
    public Material ditherMaterial;
    private RaycastHit[] _obstacleHits;
    private RaycastHit _playerHit;
    private Dictionary<MeshRenderer, DitherObj> _ditherObjs;
    private class DitherObj
    {
        public Material[] Materials;
        public bool Flag;
        public DitherObj(MeshRenderer renderer, bool flag)
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
        CameraMgr.Instance.Init(cameraTarget.rotation.eulerAngles);
    }

    private void LateUpdate()
    {
        var cameraLook = InputMgr.Instance.CameraLook;
        var cameraRotation = CameraMgr.Instance.CameraRotateViaInput(cameraLook);
        cameraTarget.rotation = cameraRotation;
    }
    
    
    private void UpdateRaycastHit()
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
            Physics.RaycastNonAlloc(cameraPos, viewDir, _obstacleHits, distance, obstacleLayer);
            foreach (var hit in _obstacleHits.Where(hit => !hit.transform.CompareTag("Player")))
            {
                var renderers = hit.transform.GetComponentsInChildren<MeshRenderer>();
                foreach (var renderer in renderers)
                {
                    if (!_ditherObjs.ContainsKey(renderer))
                    {
                        var ditherObj = new DitherObj(renderer, true);
                        _ditherObjs.Add(renderer, ditherObj);
                        
                        var tempMaterials = new Material[renderer.materials.Length];
                        for (var i = 0; i < renderer.materials.Length; i++)
                        {
                            tempMaterials[i] = ditherMaterial;
                        }
                        renderer.materials = tempMaterials;
                    }
                    else
                    {
                        _ditherObjs[renderer].Flag = true;
                    }
                }
            }
        }

        var removeDitherObjs = new List<MeshRenderer>();
        foreach (var ditherObj in _ditherObjs.Where(ditherObj => ditherObj.Value.Flag == false))
        {
            ditherObj.Key.materials = ditherObj.Value.Materials;
            removeDitherObjs.Add(ditherObj.Key);
        }

        foreach (var removeDitherObj in removeDitherObjs)
        {
            _ditherObjs.Remove(removeDitherObj);
        }
        
    }

    
}