using JLXB.Framework;
using UnityEngine;

public class PlayerMgr : Singleton<PlayerMgr>
{
    private Transform _cameraTrans;
    private Vector3 _moveInputVector;
    private Vector3 _lookInputVector;

    //最大移动速度
    private const float MaxStableSprintSpeed = 6f;
    private const float MaxStableRunSpeed = 3f;
    private const float MaxStableCrouchMoveSpeed = 2f;

    //加减速敏锐度
    private const float StableMovementSharpness = 15f;

    //转向敏锐度
    private const float OrientationSharpness = 10f;
    

    public static readonly Vector3 Gravity = new(0, -30f, 0);
    public static readonly float[] NormalCapsuleDimensions = { 0.3f, 1.82f, 0.91f };
    public static readonly float[] CrouchCapsuleDimensions = { 0.5f, 1.2f, 0.6f };

    private PlayerStateMachine _playerStateMachine;
    
    
    private PlayerMgr()
    {
    }

    public void Init(Transform cameraTrans)
    {
        _cameraTrans = cameraTrans;
        //_playerStateMachine = new PlayerStateMachine();
    }

    public void UpdateInput(Vector2 inputMovement, Vector3 planeNormal)
    {
        var moveInputVector = new Vector3(inputMovement.x, 0f, inputMovement.y);
        var cameraPlanarDirection = Vector3.ProjectOnPlane(_cameraTrans.forward, planeNormal).normalized;
        if (cameraPlanarDirection.sqrMagnitude == 0f)
        {
            cameraPlanarDirection = Vector3.ProjectOnPlane(_cameraTrans.up, planeNormal).normalized;
        }

        var cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, planeNormal);
        _moveInputVector = cameraPlanarRotation * moveInputVector;
        _lookInputVector = _moveInputVector.normalized;
    }

    public void UpdateAnimation()
    {
        var movement = _moveInputVector.magnitude;
        //_playerStateMachine.OnLogic();
        
    }

    public (bool, Quaternion) GetUpdateRotation(Vector3 characterForward, Vector3 characterUp, float deltaTime)
    {
        if (!(_lookInputVector.sqrMagnitude > 0f) || !(OrientationSharpness > 0f)) return (false, default);
        var smoothedLookInputDirection = Vector3.Slerp(characterForward, _lookInputVector,
            1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;
        return (true, Quaternion.LookRotation(smoothedLookInputDirection, characterUp));
    }

    public Vector3 GetUpdateVelocity(bool isStableOnGround, Vector3 currentVelocity, Vector3 characterUp,
        Vector3 groundNormal, float deltaTime)
    {
        if (!isStableOnGround) return currentVelocity + Gravity * deltaTime;
        // 求出玩家运动趋势相对于地表的切线方向 对玩家速度进行相对于地表情况的修正
        var directionRight = Vector3.Cross(currentVelocity, characterUp);
        var tangentVelocity = Vector3.Cross(groundNormal, directionRight).normalized * currentVelocity.magnitude;
        var inputRight = Vector3.Cross(_moveInputVector, characterUp);
        var reorientedInput = Vector3.Cross(groundNormal, inputRight).normalized * _moveInputVector.magnitude;
        var maxStableMoveSpeed = GetMaxStableMoveSpeed();
        var targetMovementVelocity = reorientedInput * maxStableMoveSpeed;
        return Vector3.Lerp(tangentVelocity, targetMovementVelocity,
            1 - Mathf.Exp(-StableMovementSharpness * deltaTime));
    }

    private static float GetMaxStableMoveSpeed()
    {
        return MaxStableRunSpeed;
    }
}