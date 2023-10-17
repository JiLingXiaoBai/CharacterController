using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float TopClamp = 70.0f;
    private const float BottomClamp = -30.0f;
    private const float Sensitivity = 0.12f;
    private const float CameraThreshold = 0.01f;
    // 俯仰角
    private float _cameraTargetPitch;
    // 偏航角
    private float _cameraTargetYaw;
    
    
    public Transform cameraTarget;
    public LayerMask obstacleLayer;
    public Material ditherMaterial;
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

        var targetEulerAngles = cameraTarget.rotation.eulerAngles;
        _cameraTargetYaw = targetEulerAngles.y;
        _cameraTargetPitch = targetEulerAngles.x;
    }

    private void LateUpdate()
    {
        var cameraLook = InputMgr.Instance.CameraLook;
        
        if (cameraLook.sqrMagnitude >= CameraThreshold)
        {
            _cameraTargetPitch -= cameraLook.y * Sensitivity;
            _cameraTargetYaw += cameraLook.x * Sensitivity;
        }
        _cameraTargetPitch = ClampAngle(_cameraTargetPitch, BottomClamp, TopClamp);
        _cameraTargetYaw = ClampAngle(_cameraTargetYaw, float.MinValue, float.MaxValue);
        cameraTarget.rotation = Quaternion.Euler(_cameraTargetPitch, _cameraTargetYaw, 0f);
        
    }

    private void FixedUpdate()
    {
        UpdateDitherObstacles();
    }
    
    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
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