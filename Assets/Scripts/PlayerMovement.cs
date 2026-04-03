using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private float _groundCheckSphereRadius = 0.5f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundCheckPosition;
    [SerializeField] private Transform _cameraHolderX;
    [SerializeField] private Transform _camera;

    public event Action Jump;

    void Start()
    {
        InputManager.Instance.JumpInputPressed += OnJumpPressed;
    }

    private void OnJumpPressed()
    {
        // U hitInfo spremamo podatke o rajcastu
        RaycastHit hitInfo;

        // OverlapSphere provjerava postoji li na tom mjestu "ground"

        Collider[] colliders = Physics.OverlapSphere(_groundCheckPosition.position, _groundCheckSphereRadius, _groundLayer);
 
        // Ako nema nijednog collidera, izlazimo iz metode
        if (colliders.Length == 0)
        {
            return;
        }

        Jump?.Invoke();
        // ovdje prosiriti logiku i dodati jump funkcionalnost
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0, _camera.rotation.eulerAngles.y, 0);
    }

    void  FixedUpdate()
    {
        Vector3 rawDir = _camera.forward * InputManager.Instance.VerticalAxis
                       + _camera.right  * InputManager.Instance.HorizontalAxis;
        rawDir.y = 0;

        // Clamp to magnitude 1 — prevents diagonal movement being ~41% faster than cardinal
        if (rawDir.sqrMagnitude > 1f)
            rawDir = rawDir.normalized;

        _rigidbody.MovePosition(_rigidbody.position + rawDir * _movementSpeed * Time.fixedDeltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_groundCheckPosition.position, 0.5f);
    }
}