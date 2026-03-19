using System;
using UnityEngine;

public class CharacterControllerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpVelocity;

    private const float GRAVITY = -9.81f;
    private const float GROUNDED_GRAVITY_PULL = -0.1f;
    private Vector3 _velocity;
    private bool _hasJumped;

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
        Gravity();

        Vector3 directionF = _camera.forward * _movementSpeed * InputManager.Instance.VerticalAxis * Time.deltaTime;
        Vector3 directionH = _camera.right * _movementSpeed * InputManager.Instance.HorizontalAxis * Time.deltaTime;
        directionF.y = 0;
        directionH.y = 0;

        var move = directionF + directionH + _velocity;

        _characterController.Move(move);
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0, _camera.rotation.eulerAngles.y, 0);
    }

    private void Gravity()
    {
        if (_characterController.isGrounded)
        {
            if (_hasJumped)
            {
                _velocity = _jumpVelocity * Vector3.up * Time.deltaTime;
                _hasJumped = false;
            }
            else
                _velocity = Vector3.up * GROUNDED_GRAVITY_PULL;
        }
        else
        {
            _velocity += Vector3.up * GRAVITY * Time.deltaTime * Time.deltaTime;
        }
    }
}
