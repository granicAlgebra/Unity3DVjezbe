using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private Rigidbody _rigidbody;

    void Start()
    {
        InputManager.Instance.JumpInputPressed += OnJumpPressed;
    }

    private void OnJumpPressed()
    {
        Debug.Log("Jump");
        // ovdje prosiriti logiku i dodati jump funkcionalnost
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    void Update()
    {
        
    }
}
