using System;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterControllerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _camera;

    [SerializeField] private float _walkingSpeed;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _jumpVelocity;
    [SerializeField] private float _directionalRotationSpeed;
    [SerializeField] private float _slideFriction;

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
        IsGrounded = _characterController.isGrounded;
        _onSteepSlope = Vector3.Angle(Vector3.up, _hitNormal) > _characterController.slopeLimit;

        Gravity();

        float movement = _movementSpeed;
        if (InputManager.Instance.Sprint > 0.1f)
        {
            movement = _sprintSpeed;
            Debug.Log("Sprint");
        }
        else if (InputManager.Instance.Walk > 0.1f)
        {
            movement = _walkingSpeed;
            Debug.Log("Walk");
        }

        Vector3 directionF = _camera.forward * movement * InputManager.Instance.VerticalAxis;
        Vector3 directionH = _camera.right * movement * InputManager.Instance.HorizontalAxis;
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

        VelocityMagnitude = move.magnitude / Time.deltaTime;
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

    private void Jump(ref Vector3 velocity)
    {
        velocity.y = _jumpVelocity;
        JumpEvent?.Invoke();
        _hasJumped = false;
    }
}
