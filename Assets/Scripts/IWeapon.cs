using UnityEngine;

public interface IWeapon 
{
    public void Fire(Vector3 position);

    public void SetStartPosition(Vector3 position);
}
