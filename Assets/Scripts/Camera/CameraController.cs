using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTarget;
    public LayerMask obstacleLayer;
    private RaycastHit[] _raycastHits;
    

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
    
    private void Update()
    {
        
    }


    private void UpdateRaycastHit()
    {
        var cameraPos = transform.position;
        var targetPos = cameraTarget.position;
        var viewDir = (targetPos - cameraPos).normalized;
        var distance = Vector3.Distance(cameraPos, targetPos);

        var ray = new Ray(cameraPos, viewDir);
        
    }
}