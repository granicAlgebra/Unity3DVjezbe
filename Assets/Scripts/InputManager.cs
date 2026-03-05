using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public event Action JumpInputPressed;

    public float MouseX => Input.GetAxis("Mouse X");
    public float MouseY => Input.GetAxis("Mouse Y");

    public float HorizontalAxis => Input.GetAxis("Horizontal");
    public float VerticalAxis => Input.GetAxis("Vertical");

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            JumpInputPressed?.Invoke();
        }

        //HorizontalAxis = Input.GetAxis("Horizontal");
    }
}