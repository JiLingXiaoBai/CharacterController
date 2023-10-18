using KinematicCharacterController;
using UnityEngine;
using JLXB.Framework.EventCenter;
using ARPG.Input;

[RequireComponent(typeof(KinematicCharacterMotor))]
public class PlayerMovementController : MonoBehaviour, ICharacterController
{
    private Transform _cameraTrans;
    private Vector3 _moveInputVector;
    private Vector3 _lookInputVector;
    private KinematicCharacterMotor _motor;

    //最大移动速度
    private const float MaxStableSprintSpeed = 6f;
    private const float MaxStableRunSpeed = 3f;
    private const float MaxStableCrouchMoveSpeed = 2f;

    //加减速敏锐度
    private const float StableMovementSharpness = 15f;

    //转向敏锐度
    private const float OrientationSharpness = 10f;

    private static readonly Vector3 Gravity = new(0, -30f, 0);
    private static readonly float[] NormalCapsuleDimensions = { 0.3f, 1.82f, 0.91f };
    private static readonly float[] CrouchCapsuleDimensions = { 0.5f, 1.2f, 0.6f };
    

    private void Awake()
    {
        EventCenter.Register<float, float, float>(EventConst.PlayerSetCapsuleDimensions,
            (radius, height, yOffset) => _motor.SetCapsuleDimensions(radius, height, yOffset));
    }

    private void Start()
    {
        InputMgr.Instance.EnableGameplayInput();
        _motor = GetComponent<KinematicCharacterMotor>();
        _motor.CharacterController = this;
        _cameraTrans = Camera.main ? Camera.main.transform : new Camera().transform;
    }

    private void Update()
    {
        UpdateInput();
    }
    
    private void UpdateInput()
    {
        var inputMovement = InputMgr.Instance.Movement;
        var moveInputVector = new Vector3(inputMovement.x, 0f, inputMovement.y);
        var cameraPlanarDirection = Vector3.ProjectOnPlane(_cameraTrans.forward, _motor.CharacterUp).normalized;
        if (cameraPlanarDirection.sqrMagnitude == 0f)
        {
            cameraPlanarDirection = Vector3.ProjectOnPlane(_cameraTrans.up, _motor.CharacterUp).normalized;
        }

        var cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, _motor.CharacterUp);

        _moveInputVector = cameraPlanarRotation * moveInputVector;
        _lookInputVector = _moveInputVector.normalized;
    }
    

    /// <summary>
    /// 能且仅能在此处控制角色转向
    /// </summary>
    /// <param name="currentRotation">需要被设置的玩家rotation</param>
    /// <param name="deltaTime">刷新时间,默认与fixedUpdate同步,0.02s</param>
    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        if (!(_lookInputVector.sqrMagnitude > 0f) || !(OrientationSharpness > 0f)) return;
        var smoothedLookInputDirection = Vector3.Slerp(_motor.CharacterForward, _lookInputVector,
            1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;
        currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, _motor.CharacterUp);
    }

    /// <summary>
    /// 能且仅能在此处控制角色移动
    /// </summary>
    /// <param name="currentVelocity">需要被设置的玩家速度</param>
    /// <param name="deltaTime">刷新时间,默认与fixedUpdate同步,0.02s</param>
    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        // 地面检测
        if (_motor.GroundingStatus.IsStableOnGround)
        {
            // GetDirectionTangentToSurface 可以求出玩家运动趋势相对于地表的切线方向
            // 对玩家速度进行相对于地表情况的修正
            currentVelocity =
                _motor.GetDirectionTangentToSurface(currentVelocity, _motor.GroundingStatus.GroundNormal) *
                currentVelocity.magnitude;
            var inputRight = Vector3.Cross(_moveInputVector, _motor.CharacterUp);
            var reorientedInput = Vector3.Cross(_motor.GroundingStatus.GroundNormal, inputRight).normalized *
                                  _moveInputVector.magnitude;

            var maxStableMoveSpeed = GetMaxStableMoveSpeed();
            var targetMovementVelocity = reorientedInput * maxStableMoveSpeed;

            currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity,
                1 - Mathf.Exp(-StableMovementSharpness * deltaTime));
        }
        else // 在空中
        {
            currentVelocity += Gravity * deltaTime;
        }
    }

    public void BeforeCharacterUpdate(float deltaTime)
    {
    }

    public void PostGroundingUpdate(float deltaTime)
    {
    }

    public void AfterCharacterUpdate(float deltaTime)
    {
    }

    public bool IsColliderValidForCollisions(Collider coll)
    {
        return true;
    }

    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
        ref HitStabilityReport hitStabilityReport)
    {
    }

    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
        ref HitStabilityReport hitStabilityReport)
    {
    }

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
        Vector3 atCharacterPosition,
        Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
    {
    }

    public void OnDiscreteCollisionDetected(Collider hitCollider)
    {
    }


    private static float GetMaxStableMoveSpeed()
    {
        return MaxStableRunSpeed;
    }
}