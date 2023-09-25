using KinematicCharacterController;
using UnityEngine;

[RequireComponent(typeof(KinematicCharacterMotor))]
public class PlayerController : MonoBehaviour, ICharacterController
{
    private KinematicCharacterMotor _motor;

    private void Start()
    {
        InputMgr.Instance.EnableGameplayInput();
        _motor = GetComponent<KinematicCharacterMotor>();
        _motor.CharacterController = this;
        PlayerMgr.Instance.Init(Camera.main ? Camera.main.transform : new Camera().transform);
    }

    private void Update()
    {
        PlayerMgr.Instance.UpdateInput(InputMgr.Instance.Movement, _motor.CharacterUp);
    }

    private void LateUpdate()
    {
        var capsuleDimensions = PlayerMgr.Instance.UpdateAnimation();
        _motor.SetCapsuleDimensions(capsuleDimensions[0], capsuleDimensions[1], capsuleDimensions[2]);
    }

    /// <summary>
    /// 能且仅能在此处控制角色转向
    /// </summary>
    /// <param name="currentRotation">需要被设置的玩家rotation</param>
    /// <param name="deltaTime">刷新时间,默认与fixedUpdate同步,0.02s</param>
    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        var rotationTemp = PlayerMgr.Instance.GetUpdateRotation(_motor.CharacterForward, _motor.CharacterUp, deltaTime);
        if (rotationTemp.Item1)
        {
            currentRotation = rotationTemp.Item2;
        }
    }

    /// <summary>
    /// 能且仅能在此处控制角色移动
    /// </summary>
    /// <param name="currentVelocity">需要被设置的玩家速度</param>
    /// <param name="deltaTime">刷新时间,默认与fixedUpdate同步,0.02s</param>
    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        currentVelocity = PlayerMgr.Instance.GetUpdateVelocity(_motor.GroundingStatus.IsStableOnGround, currentVelocity,
            _motor.CharacterUp, _motor.GroundingStatus.GroundNormal, deltaTime);
    }

    public void BeforeCharacterUpdate(float deltaTime)
    {
    }

    public void PostGroundingUpdate(float deltaTime)
    {
        switch (_motor.GroundingStatus.IsStableOnGround)
        {
            case true when !_motor.LastGroundingStatus.IsStableOnGround:
                //OnLanded();
                break;
            case false when _motor.LastGroundingStatus.IsStableOnGround:
                //OnLeaveStableGround();
                break;
        }
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
}