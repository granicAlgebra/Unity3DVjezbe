using UnityEngine;

public class Rock : MonoBehaviour, IWeapon
{
    public LayerMask LayerMask;
    public float Speed;
    private Vector3 _direction;

    private void OnCollisionEnter(Collision collision)
    {
        this.enabled = false;
    }

    void Update()
    {
        transform.position += _direction * Speed * Time.deltaTime;
    }

    public void Fire(Vector3 position)
    {
        
        _direction = position - transform.position;
    }

    public void SetStartPosition(Vector3 position)
    {
        transform.position = position;
        enabled = true;
    }
}
