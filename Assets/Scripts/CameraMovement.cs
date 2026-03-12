using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _zOffset;

    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private float _minX = -80f;
    [SerializeField] private float _maxX = 80f;

    private float _rotationX;
    private float _rotationY;

    void Update()
    {
        _rotationX -= InputManager.Instance.MouseY;
        _rotationX = Mathf.Clamp(_rotationX, _minX, _maxX);
        _rotationY += InputManager.Instance.MouseX;

        Quaternion rotation = Quaternion.Euler(_rotationX, _rotationY, 0);

        Vector3 forward = rotation * Vector3.forward;   

        Ray ray = new Ray(_target.position, -forward);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Abs(_zOffset), _layerMask, QueryTriggerInteraction.Ignore))
            transform.SetPositionAndRotation(hit.point, rotation);
        else
            transform.SetPositionAndRotation(_target.position + (forward * _zOffset), rotation);    
    }
}
