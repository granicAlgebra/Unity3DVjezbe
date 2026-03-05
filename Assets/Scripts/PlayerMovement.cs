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


    void Start()
    {
        InputManager.Instance.JumpInputPressed += OnJumpPressed;
    }

    private void OnJumpPressed()
    {
        Debug.Log("Jump");

        // U hitInfo spremamo podatke o rajcastu
        RaycastHit hitInfo;

        // OverlapSphere provjerava postoji li na tom mjestu "ground"

        Collider[] colliders = Physics.OverlapSphere(_groundCheckPosition.position, _groundCheckSphereRadius, _groundLayer);
 
        // Ako nema nijednog collidera, izlazimo iz metode
        if (colliders.Length == 0)
        {
            return;
        }

        // ovdje prosiriti logiku i dodati jump funkcionalnost
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    void  FixedUpdate()
    {
        Vector3 directionF = transform.forward * _movementSpeed * InputManager.Instance.VerticalAxis * Time.fixedDeltaTime;
        Vector3 directionH = transform.right * _movementSpeed * InputManager.Instance.HorizontalAxis * Time.fixedDeltaTime;

        Vector3 velocity = directionF + directionH;

        _rigidbody.MovePosition(_rigidbody.position + velocity);

    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_groundCheckPosition.position, 0.5f);
    }
}