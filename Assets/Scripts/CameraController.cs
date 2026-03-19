using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _3rdPersonCamera;
    [SerializeField] private int _priority = 5;

    private bool _switch = false;

    void Start()
    {
        InputManager.Instance.RightMouseClick += OnRightMouseClick;
    }

    private void OnRightMouseClick()
    {
        _switch = !_switch;

        _3rdPersonCamera.Priority = _switch ? _priority : 0;
    }
}
