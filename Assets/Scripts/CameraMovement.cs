using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _target;

    void Update()
    {
        transform.SetPositionAndRotation(_target.position, _target.rotation);    
    }
}
