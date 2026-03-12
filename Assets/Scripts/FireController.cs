using System;
using UnityEngine;

public class FireController : MonoBehaviour
{
    [SerializeField] private PlayerCrosshair _crosshair;
    [SerializeField] private Transform _weaponStartPosition;
    [SerializeField] private GameObject _weapon;

    private void Start()
    {
        _crosshair.OnRaycastHit += Fire;
    }

    private void Fire(RaycastHit hit)
    {
        var weapon = GameObject.Instantiate(_weapon, _weaponStartPosition.position, Quaternion.identity).GetComponent<IWeapon>();
  
        weapon.Fire(hit.point);    
    }
}
