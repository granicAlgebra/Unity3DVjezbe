using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public event Action JumpInputPressed;
    public event Action LeftMouseClick;
    public event Action RightMouseClick;

    [SerializeField] private float _mouseSensitivity;
    [SerializeField] private bool _cursorVisible;

    public float MouseX;
    public float MouseY;

    // GetAxisRaw returns exact -1/0/1 with no built-in smoothing — more consistent with physics movement
    public float HorizontalAxis => Input.GetAxisRaw("Horizontal");
    public float VerticalAxis => Input.GetAxisRaw("Vertical");
    public bool Sprint => Input.GetButton("Sprint");
    public bool Walk => Input.GetButton("Walk");

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
       
        if (Input.GetMouseButtonDown(1))
        {
            RightMouseClick?.Invoke();
        }

        MouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        MouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;
    }
}