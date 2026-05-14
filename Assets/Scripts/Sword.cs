using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private int _damage;
    [SerializeField] private float _attackForce;
    [SerializeField] private float _attackHitboxRadius;
    [SerializeField] private float _throwForce;
    [SerializeField] private Transform _hitboxPosition;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Collider _collider;

    private bool _isThrown = false;
    private Rigidbody _rigidbody;

    private void OnTriggerEnter(Collider other)
    {
        if (!_isThrown)
            return;

        var ragdoll = other.GetComponentInParent<GenericRagdoll>();
        if (ragdoll == null)
            return;

        ragdoll.Die(transform.position, _throwForce * 5, 1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isThrown)
            return;

        _isThrown = false;
        transform.SetParent(collision.collider.transform);
        Destroy(_rigidbody);
        _rigidbody = null;
    }

    public void Attack(Vector3 position)
    {
        var colliders = Physics.OverlapSphere(_hitboxPosition.position, _attackHitboxRadius, _layerMask, QueryTriggerInteraction.Collide);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.TryGetComponent<Entity>(out var entity))
            {
                entity.GetParameter(ParameterType.Health);

                entity.AddToParameter(ParameterType.Health, _damage);
            }
            else if (colliders[i].gameObject.TryGetComponent<IDamageable>(out var damageable))
            {
                if (!damageable.Equals(this))
                {
                    damageable.Damage();
                }
            }
        }
    }

    public void SetStartPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SpecialAttack(Vector3 position)
    {
     
        transform.parent = null;
        _rigidbody = gameObject.AddComponent<Rigidbody>();
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        Vector3 direction = (_hitboxPosition.forward + (Vector3.up * 0.15f)).normalized;
        _rigidbody.AddForce(direction * _throwForce, ForceMode.Impulse);
        _rigidbody.AddTorque(transform.up * 100, ForceMode.Impulse);
        StartCoroutine(SwordActivationCoroutine());
    }

    IEnumerator SwordActivationCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        _isThrown = true;
    }
}
