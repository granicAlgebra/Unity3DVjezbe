using System;
using UnityEngine;

public class CharacterControllerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _groundCheckHolder;
    [SerializeField] private float _groundCheckSphereRadius;
    [SerializeField] private LayerMask _groundCheckLayer;

    [SerializeField] private float _walkingSpeed;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _jumpVelocity;
    [SerializeField] private float _directionalRotationSpeed;
    [SerializeField] private float _slideFriction;

    [SerializeField] private AnimationCurve _movementSmooth;

    private const float GRAVITY = -19.62f;
    private const float GROUNDED_GRAVITY_PULL = -0.5f;
    private Vector3 _velocity;
    private bool _hasJumped;
    private Vector3 _hitNormal;
    private bool _onSteepSlope;

    public event Action JumpEvent;
    public bool IsGrounded {  get; private set; }
    public float VelocityMagnitude { get; private set; }
    public float MaximumVelocity => _sprintSpeed;

    [SerializeField] private float _accelerationSpeed = 10f;
    private Vector3 _smoothedMoveDir;

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
        IsGrounded = CheckIsGrounded();
        _onSteepSlope = Vector3.Angle(Vector3.up, _hitNormal) > _characterController.slopeLimit;

        Gravity();

        float movement = _movementSpeed;
        if (InputManager.Instance.Sprint)
            movement = _sprintSpeed;
        else if (InputManager.Instance.Walk)
            movement = _walkingSpeed;

        Vector3 rawDir = _camera.forward * InputManager.Instance.VerticalAxis
                       + _camera.right  * InputManager.Instance.HorizontalAxis;
        rawDir.y = 0;

        if (rawDir.sqrMagnitude > 1f)
            rawDir = rawDir.normalized;

        Vector3 moveDir = _onSteepSlope ? Vector3.zero : rawDir * movement;

        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * _directionalRotationSpeed);
        }

        float t = _movementSmooth.Evaluate(VelocityMagnitude / _sprintSpeed) * _accelerationSpeed * Time.deltaTime;
        _smoothedMoveDir = Vector3.Lerp(_smoothedMoveDir, moveDir, t);

        Vector3 move = _smoothedMoveDir * Time.deltaTime;
        move.y = _velocity.y * Time.deltaTime;

        if (!(Vector3.Angle(Vector3.up, _hitNormal) <= _characterController.slopeLimit))
        {
            Vector3 slideDir = Vector3.ProjectOnPlane(Vector3.down, _hitNormal).normalized;
            move.x += slideDir.x * _slideFriction * Time.deltaTime;
            move.z += slideDir.z * _slideFriction * Time.deltaTime;
        }

        _characterController.Move(move);
        _hasJumped = false;

        VelocityMagnitude = new Vector3(move.x, 0f, move.z).magnitude / Time.deltaTime;
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _hitNormal = hit.normal;
    }

    private void Gravity()
    {
        if (IsGrounded && !_onSteepSlope)
        {
            if (_hasJumped)
            {
                Jump(ref _velocity);
            }
            else
                _velocity.y = GROUNDED_GRAVITY_PULL;
        }
        else
        {
            _velocity.y += GRAVITY * Time.deltaTime;
        }
    }

    private bool CheckIsGrounded()
    {
        return Physics.CheckSphere(_groundCheckHolder.position, _groundCheckSphereRadius, _groundCheckLayer);
    }

    private void Jump(ref Vector3 velocity)
    {
        velocity.y = _jumpVelocity;
        JumpEvent?.Invoke();
        _hasJumped = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_groundCheckHolder.position, _groundCheckSphereRadius);
    }
}
