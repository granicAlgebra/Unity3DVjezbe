using System;
using UnityEngine;

public class PlayerCrosshair : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private float _rayDistance;
    [SerializeField] private LayerMask _layerMask;
    public event Action<RaycastHit> OnRaycastHit;

    private void Start()
    {
        InputManager.Instance.LeftMouseClick += OnLeftMouseClick;
        _camera = Camera.main;
    }

    private void OnLeftMouseClick()
    {
        Debug.Log("Click");
        Ray ray = _camera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height /2));

        if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance, _layerMask, QueryTriggerInteraction.Ignore))
        {
            OnRaycastHit?.Invoke(hit);
        }
    }
}
