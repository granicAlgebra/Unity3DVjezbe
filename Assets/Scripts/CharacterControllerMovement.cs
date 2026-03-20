using System;
using UnityEngine;

public class CharacterControllerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpVelocity;
    [SerializeField] private float _directionalRotationSpeed;
    [SerializeField] private float _slideFriction;

    private const float GRAVITY = -19.62f;
    private const float GROUNDED_GRAVITY_PULL = -0.5f;
    private Vector3 _velocity;
    private bool _hasJumped;
    private Vector3 _hitNormal;
    private bool _onSteepSlope;

    void Start()
    {
        InputManager.Instance.JumpInputPressed += OnJumpPressed;
    }

    private void OnJumpPressed()
    {
        _hasJumped = true;
    }

    void Update()
    {
        _onSteepSlope = Vector3.Angle(Vector3.up, _hitNormal) > _characterController.slopeLimit;

        Gravity();

        Vector3 directionF = _camera.forward * _movementSpeed * InputManager.Instance.VerticalAxis;
        Vector3 directionH = _camera.right * _movementSpeed * InputManager.Instance.HorizontalAxis;
        directionF.y = 0;
        directionH.y = 0;

        Vector3 moveDir = _onSteepSlope ? Vector3.zero : directionF + directionH;

        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * _directionalRotationSpeed);
        }

        Vector3 move = moveDir * Time.deltaTime;
        move.y = _velocity.y * Time.deltaTime;

        if (!(Vector3.Angle(Vector3.up, _hitNormal) <= _characterController.slopeLimit))
        {
            Vector3 slideDir = Vector3.ProjectOnPlane(Vector3.down, _hitNormal).normalized;
            move.x += slideDir.x * _slideFriction * Time.deltaTime;
            move.z += slideDir.z * _slideFriction * Time.deltaTime;
        }

        _characterController.Move(move);
        _hasJumped = false;
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _hitNormal = hit.normal;
    }

    private void Gravity()
    {
        if (_characterController.isGrounded && !_onSteepSlope)
        {
            if (_hasJumped)
            {
                _velocity.y = _jumpVelocity;
                _hasJumped = false;
            }
            else
                _velocity.y = GROUNDED_GRAVITY_PULL;
        }
        else
        {
            _velocity.y += GRAVITY * Time.deltaTime;
        }
    }
}
