using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform Target;
    public List<GameObject> Weapons;

    [SerializeField] private AnimationTrigger _animationTrigger;

    private IWeapon _currentWeapon;

    private void Start()
    {
        _currentWeapon = Weapons[0].GetComponent<IWeapon>();
        _animationTrigger.OnAttackTriggered += Attack;
        _animationTrigger.OnSpecialAttackTriggered += SpecialAttack;
    }

    public void Attack()
    {
        Vector3 targetPosition = Target != null ? Target.position : Vector3.zero; 
        if (_currentWeapon == null)
        {
            var currentWeapon = Weapons.First(w => w.activeSelf);
            currentWeapon.SetActive(true);
            _currentWeapon = currentWeapon.GetComponent<IWeapon>();
        }
        _currentWeapon.Attack(targetPosition);
    }

    public void SpecialAttack()
    {
        Vector3 targetPosition = Target != null ? Target.position : Vector3.zero;
        _currentWeapon.SpecialAttack(targetPosition);
    }
}
