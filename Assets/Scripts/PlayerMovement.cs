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
        Vector3 directionF = _camera.forward * _movementSpeed * InputManager.Instance.VerticalAxis * Time.fixedDeltaTime;
        Vector3 directionH = _camera.right * _movementSpeed * InputManager.Instance.HorizontalAxis * Time.fixedDeltaTime;
        directionF.y = 0;
        directionH.y = 0;
        

        _rigidbody.MovePosition(_rigidbody.position + directionF + directionH);    
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_groundCheckPosition.position, 0.5f);
    }
}