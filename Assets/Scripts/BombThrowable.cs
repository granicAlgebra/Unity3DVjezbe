using System.Collections;
using UnityEngine;

public class BombThrowable : MonoBehaviour, IWeapon
{
    [SerializeField] private float _throwForce;
    [SerializeField] private float _activationTimer = 2f;
    [SerializeField] private Transform _hitboxPosition;

    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Collider _collider;
    private bool _hasExploded = false;

    private bool _isThrown = false;
    private Rigidbody _rigidbody;

    private void OnTriggerEnter(Collider other)
    {
        if (!_isThrown)
            return;

        var ragdoll = other.GetComponentInParent<GenericRagdoll>();
        if (ragdoll == null)
            return;

        Explode();
    }

    public void Attack(Vector3 position)
    {
        transform.parent = null;
        _rigidbody = gameObject.AddComponent<Rigidbody>();
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        Vector3 direction = (_hitboxPosition.forward + (Vector3.up * 0.15f)).normalized;
        _rigidbody.AddForce(direction * _throwForce * 0.5f, ForceMode.Impulse);
        _rigidbody.AddTorque(transform.up * 100, ForceMode.Impulse);
        StartCoroutine(BombActivationCoroutine());
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
        StartCoroutine(BombActivationCoroutine());
    }

    public void SetStartPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void Explode()
    {
        if (_hasExploded)
        {
            return;
        }
        _hasExploded = true;

        var colliders = Physics.OverlapSphere(transform.position, _explosionRadius, _layerMask, QueryTriggerInteraction.Collide);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.TryGetComponent<GenericRagdoll>(out var ragdoll))
            {
                ragdoll.Die(transform.position, _explosionForce, _explosionRadius);
            }
            if (colliders[i].gameObject.TryGetComponent<IDamageable>(out var damageable))
            {
                if (!damageable.Equals(this))
                {
                    damageable.Damage();
                }
            }
        }

        gameObject.SetActive(false);
        _collider.enabled = false;
    }

    IEnumerator BombActivationCoroutine()
    {
        yield return new WaitForSeconds(_activationTimer);
        _isThrown = true;
        Explode();
    }
}
