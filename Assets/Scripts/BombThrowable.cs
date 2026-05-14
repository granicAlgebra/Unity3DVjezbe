using System.Collections;
using UnityEngine;

public class BombThrowable : MonoBehaviour, IWeapon
{
    [SerializeField] private int _bombAmount = 3;
    [SerializeField] private float _throwForce;
    [SerializeField] private float _activationTimer = 2f;
    [SerializeField] private float _colliderOffTime = 0.2f;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Collider _collider;
    [SerializeField] private ParticleSystem _particlePrefab;
    [SerializeField] private AudioSource _sfxPrefab;
    [SerializeField] private AudioClip _explosionSFX;
   

    private bool _isThrown = false;
    private Rigidbody _rigidbody;
    private Vector3 _startPosition;
    private Transform _startParent;
    private void Start()
    {
        _startPosition = transform.localPosition;
        _startParent = transform.parent;
    }
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
        StartCoroutine(BombActivationCoroutine());
    }

    public void SpecialAttack(Vector3 position)
    {
        Attack(position);
    }

    public void SetStartPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void Explode()
    {
        VFXManager.Instance.PlayVFX(_particlePrefab, transform.position);
        SFXManager.Instance.PlaySFX(_sfxPrefab, transform.position, _explosionSFX, 2);
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

        _bombAmount--;
        if (_bombAmount < 0)
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(_rigidbody);
           transform.parent = _startParent;
           transform.localPosition = _startPosition;
        }
        _collider.enabled = false;
    }

    IEnumerator BombActivationCoroutine()
    {
        _collider.enabled = false;
        transform.parent = null;
        _rigidbody = gameObject.AddComponent<Rigidbody>();
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        Vector3 direction = (_playerTransform.forward + (Vector3.up * 0.15f)).normalized;
        _rigidbody.AddForce(direction * _throwForce * 0.5f, ForceMode.Impulse);
        yield return new WaitForSeconds(_colliderOffTime);
        _collider.enabled = true;
        yield return new WaitForSeconds(_activationTimer);
        _isThrown = true;
        Explode();
    }
}
