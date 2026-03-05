using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public event Action JumpInputPressed;
    public event Action LeftMouseClick;

    [SerializeField] private float _mouseSensitivity;
    [SerializeField] private bool _cursorVisible;

    public float MouseX;
    public float MouseY;

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

        Cursor.visible = _cursorVisible;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            JumpInputPressed?.Invoke();
        }

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseClick?.Invoke();
        }

        MouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        MouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        //Horizon => Input.GetAxis("Mouse Y")talAxis = Input.GetAxis("Horizontal");
    }
}