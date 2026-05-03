using UnityEngine;

public interface IWeapon 
{
    public void Attack(Vector3 position);
    public void SpecialAttack(Vector3 position);
    public void SetStartPosition(Vector3 position);
}
